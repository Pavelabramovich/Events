using Microsoft.AspNetCore.Mvc;
using Events.Application.Dto;
using Events.WebApi.Authentication;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Events.Application.UseCases;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserUseCases.GetAll _getAllUseCase;
    private readonly UserUseCases.GetAllWithParticipants _getAllWithParticipantsUseCase;
    private readonly UserUseCases.GetById _getByIdUseCase;
    private readonly UserUseCases.GetByLogin _getByLoginUseCase;
    private readonly UserUseCases.GetParticipantsById _getParticipantsByIdUseCase;
    private readonly UserUseCases.GetPage _getPageUseCase;
    private readonly UserUseCases.Create _createUseCase;
    private readonly UserUseCases.Update _updateUseCase;
    private readonly UserUseCases.Remove _removeUseCase;

    private readonly AuthenticateUseCase _authenticateUseCase;
    private readonly RefreshUseCase _refreshUseCase;


    public UsersController(
        UserUseCases.GetAll getAllUseCase,
        UserUseCases.GetAllWithParticipants getAllWithParticipantsUseCase,
        UserUseCases.GetById getByIdUseCase,
        UserUseCases.GetByLogin getByLoginUseCase,
        UserUseCases.GetParticipantsById getParticipantsByIdUseCase,
        UserUseCases.GetPage getPageUseCase,
        UserUseCases.Create createUseCase,
        UserUseCases.Update updateUseCase,
        UserUseCases.Remove removeUseCase,
        
        AuthenticateUseCase authenticateUseCase,
        RefreshUseCase refreshUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _getAllWithParticipantsUseCase = getAllWithParticipantsUseCase;
        _getByIdUseCase = getByIdUseCase;
        _getByLoginUseCase = getByLoginUseCase;
        _getParticipantsByIdUseCase = getParticipantsByIdUseCase;
        _getPageUseCase = getPageUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _removeUseCase = removeUseCase;

        _authenticateUseCase = authenticateUseCase;
        _refreshUseCase = refreshUseCase;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWithoutParticipantsDto>>> GetUsers()
    {
        var users = await _getAllUseCase.ExecuteAsync();
        return Ok(users);
    }

    [HttpGet("participants")]
    public async Task<ActionResult<IEnumerable<UserWithParticipantsDto>>> GetUsersWithParticipants()
    {
        var users = await _getAllWithParticipantsUseCase.ExecuteAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserWithoutParticipantsDto>> GetUser(int id)
    {
        var user = await _getByIdUseCase.ExecuteAsync(id);

        if (user is null)
            return NotFound();

        return user;
    }

    [HttpGet("{id:int}/participants")]
    public async Task<ActionResult<IEnumerable<UserWithParticipantsDto>>> GetUserParticipants(int id)
    {
        var participants = await _getParticipantsByIdUseCase.ExecuteAsync(id);
        return Ok(participants);
    }

    [HttpGet("page/{pageNum:int}-of-{pageSize:int}")]
    public async Task<ActionResult<IEnumerable<UserWithoutParticipantsDto>>> GetUsersPage(int pageNum, int pageSize)
    {
        var eventsPage = await _getPageUseCase.ExecuteAsync(pageNum, pageSize);
        return Ok(eventsPage);
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UserWithParticipantsDto>> PostUser(UserCreatingDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try 
        { 
            await _createUseCase.ExecuteAsync(userDto);
            var newUser = _getByLoginUseCase.Execute(userDto.Login)!;

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutUser(UserWithoutParticipantsDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _updateUseCase.ExecuteAsync(userDto);
            return NoContent();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _removeUseCase.ExecuteAsync(id);
            return NoContent();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }


    [HttpPost("authenticate-user")]
    public async Task<IActionResult> AuthenticateAsync(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newTokens = await _authenticateUseCase.ExecuteAsync(userLoginDto);
            return Ok(newTokens);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(Tokens tokens)
    {
        try
        {
            var newTokens = await _refreshUseCase.ExecuteAsync(tokens);
            return Ok(newTokens);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
