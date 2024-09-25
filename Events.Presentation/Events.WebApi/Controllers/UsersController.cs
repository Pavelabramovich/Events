using Microsoft.AspNetCore.Mvc;
using Events.Application.Dto;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using Events.Application.UseCases;
using Events.Application.Exceptions;
using FluentValidation;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly GetAllUsersUseCase _getAllUseCase; 
    private readonly GetUserByIdUseCase _getByIdUseCase;
    private readonly GetUserByLoginUseCase _getByLoginUseCase;
    private readonly GetUsersPageUseCase _getPageUseCase;
    private readonly CreateUserUseCase _createUseCase;
    private readonly UpdateUserUseCase _updateUseCase;
    private readonly RemoveUserUseCase _removeUseCase;

    private readonly AuthenticateUseCase _authenticateUseCase;
    private readonly RefreshUseCase _refreshUseCase;


    public UsersController(
        GetAllUsersUseCase getAllUseCase,
        
        GetUserByIdUseCase getByIdUseCase,
        GetUserByLoginUseCase getByLoginUseCase,
        
        GetUsersPageUseCase getPageUseCase,
        CreateUserUseCase createUseCase,
        UpdateUserUseCase updateUseCase,
        RemoveUserUseCase removeUseCase,
        
        AuthenticateUseCase authenticateUseCase,
        RefreshUseCase refreshUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _getByIdUseCase = getByIdUseCase;
        _getByLoginUseCase = getByLoginUseCase;
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserWithoutParticipantsDto>> GetUser(int id)
    {
        return await _getByIdUseCase.ExecuteAsync(id);
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
        await _createUseCase.ExecuteAsync(userDto);
        var newUser = _getByLoginUseCase.Execute(userDto.Login)!;

        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutUser(UserWithoutParticipantsDto userDto)
    {   
        await _updateUseCase.ExecuteAsync(userDto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _removeUseCase.ExecuteAsync(id);
        return NoContent();
    }


    [HttpPost("authenticate-user")]
    public async Task<IActionResult> AuthenticateAsync(UserLoginDto userLoginDto)
    {   
        var newTokens = await _authenticateUseCase.ExecuteAsync(userLoginDto);
        return Ok(newTokens);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(TokensDto tokens)
    {
        var newTokens = await _refreshUseCase.ExecuteAsync(tokens);
        return Ok(newTokens);
    }
}
