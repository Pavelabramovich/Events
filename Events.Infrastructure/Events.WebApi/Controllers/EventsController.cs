using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Events.Entities;
using Events.WebApi.Db;
using Events.WebApi.Dto;
using Events.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly EventsContext _context;
    private readonly IMapper _mapper;


    public EventsController(EventsContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEvents()
    {
        return await _context
            .Events
            .Include(e => e.Users)
            .ProjectTo<EventWithParticipantsDto>(_mapper)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<EventWithoutParticipantsDto>> GetEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);

        if (@event is null)
            return NotFound();

        return _mapper.Map<EventWithoutParticipantsDto>(@event);
    }

    [HttpPost]
    [Authorize("Admin")]
    public async Task<ActionResult<Event>> PostEvent(EventCreatingDto eventDto)
    {
        var @event = _mapper.Map<Event>(eventDto);

        var eventEntry = _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = eventEntry.Entity.Id }, _mapper.Map<EventWithoutParticipantsDto>(eventEntry.Entity));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvent(int id, EventWithoutParticipantsDto eventDto)
    {
        if (id != eventDto.Id)
            return BadRequest();
        
        var @event = await _context.Events.FindAsync(eventDto.Id);

        if (@event is null)
            return NotFound();

        Event eventToReplace = _mapper.Map(eventDto, @event);

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);

        if (@event is null)
            return NotFound();
        
        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}
