using Microsoft.AspNetCore.Mvc;
using Events.Domain;
using Events.Application.Dto;
using Events.Application.UseCases;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly EventUseCases.GetAll _getAllUseCase;
    private readonly EventUseCases.GetById _getByIdUseCase;
    private readonly EventUseCases.GetByName _getByNameUseCase;
    private readonly EventUseCases.GetAllWithParticipants _getAllWithParticipantsUseCase;
    private readonly EventUseCases.GetParticipantsById _getParticipantsByIdUseCase;
    private readonly EventUseCases.GetPage _getPageUseCase;
    private readonly EventUseCases.Create _createUseCase;
    private readonly EventUseCases.Update _updateUseCase;
    private readonly EventUseCases.UpdatePaticipation _updatePaticipationUseCase;
    private readonly EventUseCases.Remove _removeUseCase;
    private readonly EventUseCases.RemoveParticipation _removeParticipationUseCase;


    public EventsController(
        EventUseCases.GetAll getAllUseCase,
        EventUseCases.GetById getByIdUseCase,
        EventUseCases.GetByName getByNameUseCase,
        EventUseCases.GetAllWithParticipants getAllWithParticipantsUseCase,
        EventUseCases.GetParticipantsById getParticipantsByIdUseCase,
        EventUseCases.GetPage getPageUseCase,
        EventUseCases.Create createUseCase,
        EventUseCases.Update updateUseCase,
        EventUseCases.UpdatePaticipation updatePaticipationUseCase,
        EventUseCases.Remove removeUseCase,
        EventUseCases.RemoveParticipation removeParticipationUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _getByIdUseCase = getByIdUseCase;
        _getByNameUseCase = getByNameUseCase;
        _getAllWithParticipantsUseCase = getAllWithParticipantsUseCase;
        _getParticipantsByIdUseCase = getParticipantsByIdUseCase;
        _getPageUseCase = getPageUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _updatePaticipationUseCase = updatePaticipationUseCase;
        _removeUseCase = removeUseCase;
        _removeParticipationUseCase = removeParticipationUseCase;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventWithoutParticipantsDto>>> GetEvents()
    {
        var events = await _getAllUseCase.ExecuteAsync();
        return Ok(events);
    }

    [HttpGet("participanst")]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEventsWithParticipants()
    {
        var events = await _getAllWithParticipantsUseCase.ExecuteAsync();
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventWithoutParticipantsDto>> GetEvent(int id)
    {
        var @event = await _getByIdUseCase.ExecuteAsync(id);

        return @event 
            ?? throw new ValidationException();
    }

    [HttpGet("page/{pageNum:int}-of-{pageSize:int}")]
    public async Task<ActionResult<IEnumerable<EventWithoutParticipantsDto>>> GetEventsPage(int pageNum, int pageSize)
    {
        var events = await _getPageUseCase.ExecuteAsync(pageNum, pageSize);
        return Ok(events);
    }


    [HttpGet("{id:int}/participants")]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEventParticipants(int id)
    {
        var participants = await _getParticipantsByIdUseCase.ExecuteAsync(id);
        return Ok(participants);
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Event>> PostEvent(EventCreatingDto eventDto)
    {
        try
        {
            await _createUseCase.ExecuteAsync(eventDto);
        }
        catch (ValidationException exception)
        {
            return BadRequest(exception.Message);
        }

        var newEvent = await _getByNameUseCase.ExecuteAsync(eventDto.Name);

        return CreatedAtAction(nameof(GetEvent), new { id = newEvent!.Id }, newEvent);
    }


    [HttpPost("{eventId:int}/participants/{userId:int}")]
    [Authorize]
    public async Task<IActionResult> PostEventParticipant(int eventId, int userId)
    {
        try
        {
            await _updatePaticipationUseCase.ExecuteAsync(eventId, userId);
        }
        catch (ValidationException exception)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{eventId:int}/participants/{userId:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteEventParticipant(int eventId, int userId)
    {
        try
        {
            await _removeParticipationUseCase.ExecuteAsync(eventId, userId);
        }
        catch (ValidationException exception)
        {
            return BadRequest();
        }

        return NoContent();
    }


    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutEvent(EventWithoutParticipantsDto eventDto)
    {
        try
        {
            await _updateUseCase.ExecuteAsync(eventDto); 
        }
        catch (ValidationException exception)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        try
        {
            await _removeUseCase.ExecuteAsync(id);
        }
        catch (ValidationException exception)
        {
            return BadRequest();
        }

        return NoContent();
    }
}
