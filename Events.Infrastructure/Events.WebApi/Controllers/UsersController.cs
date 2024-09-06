using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Events.WebApi.Dto;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Events.DataBase;
using Events.Domain.Entities;

using DomainClaim = Events.Domain.Entities.Claim;
using SystemClaim = System.Security.Claims.Claim;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWorkWithTokens _unitOfWork;
    private readonly IJwtManagerRepository _jwtManager;
    private readonly IMapper _mapper;


    public UsersController(IUnitOfWorkWithTokens unitOfWorkWithTokens, IMapper mapper, IJwtManagerRepository jwtManager)
    {
        _unitOfWork = unitOfWorkWithTokens;
        _mapper = mapper;
        _jwtManager = jwtManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWithoutParticipantsDto>>> GetUsers()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync().ToListAsync();

        return Ok(users.Select(u => _mapper.Map<UserWithoutParticipantsDto>(u)));
    }

    [HttpGet("participants")]
    public async Task<ActionResult<IEnumerable<UserWithParticipantsDto>>> GetUsersWithParticipants()
    {
        var users = await _unitOfWork.UserRepository.GetAllWithParticipationsAsync().ToListAsync();

        return Ok(users.Select(u => _mapper.Map<UserWithParticipantsDto>(u)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserWithoutParticipantsDto>> GetUser(int id)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(id);

        if (user is null)
            return NotFound();
        
        return _mapper.Map<UserWithoutParticipantsDto>(user);
    }

    [HttpGet("{id}/participants")]
    public async Task<ActionResult<IEnumerable<UserWithParticipantsDto>>> GetUserParticipants(int id)
    {
        var participants = await _unitOfWork.UserRepository.GetUserEventsAsync(id).ToArrayAsync();

        return Ok(participants.Select(p => _mapper.Map<ParticipantWithoutUserDto>(p)));
    }

    [HttpGet("page/{pageNum}-of-{pageSize}")]
    public async Task<ActionResult<IEnumerable<UserWithoutParticipantsDto>>> GetUsersPage(int pageNum, int pageSize)
    {
        int skip = pageNum * pageSize;
        int take = pageSize;

        var events = await _unitOfWork.UserRepository.PageAllAsync(skip, take).ToArrayAsync();

        return Ok(events.Select(u => _mapper.Map<UserWithoutParticipantsDto>(u)));
    }


    [HttpPost]
    public async Task<ActionResult<UserWithParticipantsDto>> PostUser(UserCreatingDto userDto)
    {
        User user = _mapper.Map<User>(userDto);

        _unitOfWork.UserRepository.Add(user);
        await _unitOfWork.SaveChangesAsync();

        var newUser = _unitOfWork.UserRepository.FindByName(userDto.Name)!;

        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, _mapper.Map<EventWithoutParticipantsDto>(user with { Id = newUser.Id }));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserWithoutParticipantsDto userDto)
    {
        if (id != userDto.Id)
            return BadRequest();

        var user = await _unitOfWork.UserRepository.FindByIdAsync(userDto.Id);

        if (user is null)
            return NotFound();

        var userToReplace = _mapper.Map<UserWithoutParticipantsDto, User>(userDto, user);

        _unitOfWork.UserRepository.Update(userToReplace);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(id);

        if (user is null)
            return NotFound();

        _unitOfWork.UserRepository.Remove(id);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }


    [HttpPost("authenticate-user")]
    public async Task<IActionResult> AuthenticateAsync(UserLoginDto userLoginDto)
    {
        bool isUserValid = await _unitOfWork.UserRepository.AuthenticateAsync(userLoginDto.Login, userLoginDto.HashedPassword);

        if (!isUserValid)
            return Unauthorized("Invalid username or password...");

        var user = await _unitOfWork.UserRepository.FindByLoginAsync(userLoginDto.Login);

        var userRoles = await _unitOfWork.RoleRepository.GetUserRolesAsync(user!.Id).ToArrayAsync();
        var userClaims = await _unitOfWork.ClaimRepository.GetUserClaimsAsync(user!.Id).ToArrayAsync();

        var claimsFromRoles = userRoles.Select(r => new SystemClaim(ClaimTypes.Role, r.Name));
        var claimsFromClaims = userClaims.Select(c => new SystemClaim(c.Type, c.Value));

        SystemClaim[] claims = [.. claimsFromRoles, .. claimsFromClaims];

        var tokens = _jwtManager.GenerateToken(user.Id, claims);

        if (tokens is null)
            return Unauthorized("Invalid Attempt..");

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Value = tokens.RefreshToken,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        _unitOfWork.RefreshTokenRepository.UpsertUserRefreshToken(refreshToken);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return Ok(tokens);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(Tokens tokens)
    {
        var principal = _jwtManager.GetPrincipalFromExpiredToken(tokens.AccessToken);

        var claims = principal.Claims;

        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!;
        int userId = int.Parse(userIdClaim.Value);

        var savedRefreshToken = await _unitOfWork.RefreshTokenRepository.GetSavedRefreshTokedAsync(userId, tokens.RefreshToken);

        if (savedRefreshToken is not null && savedRefreshToken.Value != tokens.RefreshToken)
            return Unauthorized("Invalid attempt!");
        
        var newTokens = _jwtManager.GenerateRefreshToken(userId, claims.ToArray());

        if (newTokens is null)
            return Unauthorized("Invalid attempt!");
        
        var newRefreshToken = new RefreshToken
        {
            UserId = userId,
            Value = newTokens.RefreshToken,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        _unitOfWork.RefreshTokenRepository.UpsertUserRefreshToken(newRefreshToken);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return Ok(newTokens);
    }
}
