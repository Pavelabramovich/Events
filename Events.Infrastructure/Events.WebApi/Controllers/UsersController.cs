//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using Events.Domain_Entities;
//using Events.WebApi.Db;
//using Events.WebApi.Dto;
//using Events.WebApi.Extensions;
//using Events.WebApi.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;


//namespace Events.WebApi.Controllers;


//[ApiController]
//[Route("api/[controller]")]
//public class UsersController : ControllerBase
//{
//    private readonly EventsContext _context;
//    private readonly IMapper _mapper;
//    private readonly IJWTManagerRepository _jWTManager;
//    private readonly IUserServiceRepository _userServiceRepository;


//    public UsersController(EventsContext context, IMapper mapper, IJWTManagerRepository jWTManager, IUserServiceRepository userServiceRepository)
//    {
//        _context = context;
//        _mapper = mapper;
//        _jWTManager = jWTManager;
//        _userServiceRepository = userServiceRepository;
//    }

//    // GET: api/Users
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
//    {
//        return await _context
//            .Users
//            .ProjectTo<UserDto>(_mapper)
//            .ToListAsync();
//    }

//    // GET: api/Users/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<UserDto>> GetUser(int id)
//    {
//        var user = await _context.Users.FindAsync(id);

//        if (user == null)
//        {
//            return NotFound();
//        }

//        return _mapper.Map<UserDto>(user);
//    }

//    // PUT: api/Users/5
//    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//    [HttpPut("{id}")]
//    public async Task<IActionResult> PutUser(int id, UserDto userDto)
//    {
//        if (id != userDto.Id)
//            return BadRequest();

//        var user = await _context.Users.FindAsync(id);

//        if (user is null)
//            return NotFound();

//        var userToReplace = _mapper.Map<UserDto, User>(userDto, user);

//        _context.Entry(user).CurrentValues.SetValues(userToReplace);

//        try
//        {
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException) when (!UserExists(id))
//        {
//            return NotFound();
//        }

//        return NoContent();
//    }

//    // POST: api/Users
//    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//    [HttpPost]
//    public async Task<ActionResult<UserDto>> PostUser(UserCreatingDto userDto)
//    {
//        User user = _mapper.Map<User>(userDto);

//        var userEntry = _context.Users.Add(user);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, _mapper.Map<UserDto>(userEntry.Entity));
//    }

//    // DELETE: api/Users/5
//    [HttpDelete("{id}")]
//    public async Task<IActionResult> DeleteUser(int id)
//    {
//        var user = await _context.Users.FindAsync(id);
//        if (user == null)
//        {
//            return NotFound();
//        }

//        _context.Users.Remove(user);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }


//    [AllowAnonymous]
//    [HttpPost]
//    [Route("authenticate-user")]
//    public async Task<IActionResult> AuthenticateAsync(UserLoginDto usersdata)
//    {
//        var validUser = await _userServiceRepository.IsValidUserAsync(usersdata);

//        if (!validUser)
//            return Unauthorized("Invalid username or password...");


//        var user = _context.Users.First(u => u.Email == usersdata.Login);
//        var claims = _context.UserClaims.Where(c => c.UserId == user.Id);


//        var tokens = _jWTManager.GenerateToken(usersdata.Login, claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToArray());

//        if (tokens == null)
//        {
//            return Unauthorized("Invalid Attempt..");
//        }

//        var refreshToken = new RefreshToken
//        { 
//            UserId = user.Id,
//            Value = tokens.RefreshToken,
//            Expires = DateTime.UtcNow.AddDays(30)
//        };

//        _userServiceRepository.UpsertUserRefreshToken(refreshToken);
//        return Ok(tokens);
//    }

//    [AllowAnonymous]
//    [HttpPost]
//    [Route("refresh-token")]
//    public IActionResult Refresh(Tokens tokens)
//    {
//        var principal = _jWTManager.GetPrincipalFromExpiredToken(tokens.AccessToken);
//        var email = principal.Identity?.Name;
//        var claims = principal.Claims;

//        var savedRefreshToken = _userServiceRepository.GetSavedRefreshToken(email, tokens.RefreshToken);

//        if (savedRefreshToken is not null && savedRefreshToken.Value != tokens.RefreshToken)
//        {
//            return Unauthorized("Invalid attempt!");
//        }

//        var newJwtToken = _jWTManager.GenerateRefreshToken(email, claims.ToArray());

//        if (newJwtToken == null)
//        {
//            return Unauthorized("Invalid attempt!");
//        }


//        int userId = _context.Users.First(u => u.Email == email).Id;


//        var newRefreshToken = new RefreshToken
//        { 
//            UserId = userId,
//            Value = newJwtToken.RefreshToken,
//            Expires = DateTime.UtcNow.AddDays(30)
//        };

//        _userServiceRepository.UpsertUserRefreshToken(newRefreshToken);

//        return Ok(newJwtToken);
//    }





//    private bool UserExists(int id)
//    {
//        return _context.Users.Any(e => e.Id == id);
//    }
//}
