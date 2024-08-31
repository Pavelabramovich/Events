using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Events.Entities;
using Events.WebApi.Db;
using Events.WebApi.Dto;


namespace Events.WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly EventsContext _context;

    public EventsController(EventsContext context)
    {
        _context = context;
    }

    // GET: api/Events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEvents()
    {
        return await _context
            .Events
            .Include(e => e.Users)
            .Select(e => ToEventWithParticipantsDto(e))
            .ToListAsync();
    }

    // GET: api/Events/5
    [HttpGet("{id}")]
    public async Task<ActionResult<EventWithoutParticipantsDto>> GetEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);

        if (@event is null)
            return NotFound();

        return ToEventWithoutParticipantsDto(@event);
    }

    // PUT: api/Events/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvent(int id, EventWithoutParticipantsDto eventDto)
    {
        if (id != eventDto.Id)
            return BadRequest();
        
        var @event = await _context.Events.FindAsync(eventDto.Id);

        if (@event is null)
            return NotFound();

        var eventToReplace = @event with
        {
            Name = eventDto.Name,
            Description = eventDto.Description,
            DateTime = eventDto.DateTime,
            MaxPeopleCount = eventDto.MaxPeopleCount,
            Category = eventDto.Category,
            Address = eventDto.Address,
            ImagePath = eventDto.ImagePath,
        };

        _context.Entry(@event).CurrentValues.SetValues(eventToReplace);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!EventExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Events
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Event>> PostEvent(EventToAddDto eventDto)
    {
        var @event = new Event()
        {
            Name = eventDto.Name,
            Description = eventDto.Description,
            DateTime = eventDto.DateTime,
            MaxPeopleCount = eventDto.MaxPeopleCount,
            Category = eventDto.Category,
            Address = eventDto.Address,
            ImagePath = eventDto.ImagePath,
        };

        var eventEntry = _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = eventEntry.Entity.Id }, ToEventWithoutParticipantsDto(eventEntry.Entity));
    }

    // DELETE: api/Events/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }

    private static EventWithParticipantsDto ToEventWithParticipantsDto(Event @event) => new()
    {
        Id = @event.Id,
        Name = @event.Name,
        Description = @event.Description,
        DateTime = @event.DateTime,
        Address = @event.Address,
        Category = @event.Category,
        MaxPeopleCount = @event.MaxPeopleCount,
        ImagePath = @event.ImagePath,
        Participants = [.. @event.Participants.Select(p => ToParticipantWithoutEventDto(p))],
    };

    private static EventWithoutParticipantsDto ToEventWithoutParticipantsDto(Event @event) => new()
    {
        Id = @event.Id,
        Name = @event.Name,
        Description = @event.Description,
        DateTime = @event.DateTime,
        Address = @event.Address,
        Category = @event.Category,
        MaxPeopleCount = @event.MaxPeopleCount,
        ImagePath = @event.ImagePath
    };

    private static ParticipantWithoutEventDto ToParticipantWithoutEventDto(Participation participation) => new()
    {
        UserId = participation.UserId,
        UserName = participation.User.Name,
        UserSurname = participation.User.Surname,
        RegistrationTime = participation.RegistrationTime,
    };
}
