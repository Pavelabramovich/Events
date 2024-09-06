using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Events.Domain.Entities;
using Events.WebApi.Dto;
using Events.Domain;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public EventsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventWithoutParticipantsDto>>> GetEvents()
    {
        var events = await _unitOfWork.EventRepository.GetAllAsync().ToListAsync();

        return Ok(events.Select(e => _mapper.Map<EventWithoutParticipantsDto>(e)));
    }

    [HttpGet("participanst")]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEventsWithParticipants()
    {
        var events = await _unitOfWork.EventRepository.GetAllWithParticipationsAsync().ToListAsync();

        return Ok(events.Select(e => _mapper.Map<EventWithParticipantsDto>(e)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventWithoutParticipantsDto>> GetEvent(int id)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(id);

        if (@event is null)
            return NotFound();

        return _mapper.Map<EventWithoutParticipantsDto>(@event);
    }

    [HttpGet("page/{pageNum}-of-{pageSize}")]
    public async Task<ActionResult<IEnumerable<EventWithoutParticipantsDto>>> GetEventsPage(int pageNum, int pageSize)
    {
        int skip = pageNum * pageSize;
        int take = pageSize;

        var events = await _unitOfWork.EventRepository.PageAllAsync(skip, take).ToArrayAsync();

        return Ok(events.Select(e => _mapper.Map<EventWithoutParticipantsDto>(e)));
    }


    [HttpGet("{id}/participants")]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEventParticipants(int id)
    {
        var participants = await _unitOfWork.EventRepository.GetEventParticipationsAsync(id).ToArrayAsync();

        return Ok(participants.Select(p => _mapper.Map<ParticipantWithoutEventDto>(p)));
    }


    [HttpPost]
    public async Task<ActionResult<Event>> PostEvent(EventCreatingDto eventDto)
    {
        var @event = _mapper.Map<Event>(eventDto);

        _unitOfWork.EventRepository.Add(@event);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        var newEvent = _unitOfWork.EventRepository.FindByName(eventDto.Name)!;

        return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, _mapper.Map<EventWithoutParticipantsDto>(@event with { Id = newEvent.Id }));
    }


    [HttpPost("{eventId}/participants/{userId}")]
    public async Task<IActionResult> PostEventParticipant(int eventId, int userId)
    {
        _unitOfWork.EventRepository.AddParticipant(eventId, userId);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{eventId}/participants/{userId}")]
    public async Task<IActionResult> DeleteEventParticipant(int eventId, int userId)
    {
        _unitOfWork.EventRepository.RemoveParticipant(eventId, userId);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }


    [HttpPut]
    public async Task<IActionResult> PutEvent(EventWithoutParticipantsDto eventDto)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(eventDto.Id);

        if (@event is null)
            return NotFound();

        Event eventToReplace = _mapper.Map(eventDto, @event);

        _unitOfWork.EventRepository.Update(eventToReplace);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var @event = await _unitOfWork.EventRepository.FindByIdAsync(id);

        if (@event is null)
            return NotFound();

        _unitOfWork.EventRepository.Remove(@event.Id);

        if (!await _unitOfWork.SaveChangesAsync())
            return BadRequest();

        return NoContent();
    }
}
