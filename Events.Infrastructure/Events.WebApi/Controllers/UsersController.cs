using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Events.Entities;
using Events.WebApi.Db;
using Events.WebApi.Dto;


namespace Events.WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly EventsContext _context;

    public UsersController(EventsContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return await _context
            .Users
            .Select(u => ToUserDto(u))
            .ToListAsync();
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return ToUserDto(user);
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, UserDto userDto)
    {
        if (id != userDto.Id)
            return BadRequest();

        var user = await _context.Users.FindAsync(id);

        if (user is null)
            return NotFound();

        var userToReplace = user with
        {
            Name = userDto.Name,
            Surname = userDto.Surname,
            DateOfBirth = userDto.DateOfBirth,
            Email = userDto.Email,
        };

        _context.Entry(user).CurrentValues.SetValues(userToReplace);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserAddDto userDto)
    {
        User user = new()
        {
            Name = userDto.Name,
            Email = userDto.Email,
            DateOfBirth = userDto.DateOfBirth,
            Surname = userDto.Surname,
            Password = userDto.Password
        };

        var userEntry = _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, ToUserDto(userEntry.Entity));
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }


    private static ParticipantWithoutUserDto ToParticipantWithoutEventDto(Participation participation) => new()
    {
        EventId = participation.EventId,
        EventName = participation.Event.Name,
        EventDescription = participation.Event.Description,
        RegistrationTime = participation.RegistrationTime,
    };

    private static UserDto ToUserDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        DateOfBirth = user.DateOfBirth,
        Email = user.Email,
        Surname = user.Surname,
        Password = user.Password,
    };
}
