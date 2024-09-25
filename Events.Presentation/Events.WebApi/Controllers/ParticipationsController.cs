using Events.Application.Dto;
using Events.Application.Exceptions;
using Events.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Events.WebApi.Controllers;


[Route("api/participations")]
[ApiController]
public class ParticipationsController : ControllerBase
{
    private readonly GetAllEventsWithParticipantsUseCase _getAllEventsWithParticipantsUseCase;
    private readonly GetAllUsersWithParticipationsUseCase _getAllUsersWithParticipantsUseCase;
    private readonly GetParticipantsByEventIdUseCase _getParticipantsByEventIdUseCase;
    private readonly GetParticipationsByUserIdUseCase _getParticipantsByUserIdUseCase;
    private readonly UpdateEventParticipationUseCase _updatePaticipationUseCase;
    private readonly RemoveEventParticipationUseCase _removeParticipationUseCase;


    public ParticipationsController(
        GetAllEventsWithParticipantsUseCase getAllEventsWithParticipantsUseCase,
        GetAllUsersWithParticipationsUseCase getAllUsersWithParticipantsUseCase,
        GetParticipantsByEventIdUseCase getParticipantsByEventIdUseCase,
        GetParticipationsByUserIdUseCase getParticipantsByUserIdUseCase,
        UpdateEventParticipationUseCase updateParticipationUseCase,
        RemoveEventParticipationUseCase removeParticipationUseCase)
    {
        _getAllEventsWithParticipantsUseCase = getAllEventsWithParticipantsUseCase;
        _getAllUsersWithParticipantsUseCase = getAllUsersWithParticipantsUseCase;
        _getParticipantsByEventIdUseCase = getParticipantsByEventIdUseCase;
        _getParticipantsByUserIdUseCase = getParticipantsByUserIdUseCase;
        _updatePaticipationUseCase = updateParticipationUseCase;
        _removeParticipationUseCase = removeParticipationUseCase;
    }


    [HttpGet("with-events")]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEventsWithParticipants()
    {
        var events = await _getAllEventsWithParticipantsUseCase.ExecuteAsync();
        return Ok(events);
    }

    [HttpGet("with-users")]
    public async Task<ActionResult<IEnumerable<UserWithParticipantsDto>>> GetUsersWithParticipants()
    {
        var users = await _getAllUsersWithParticipantsUseCase.ExecuteAsync();
        return Ok(users);
    }

    [HttpGet("event-{id:int}")]
    public async Task<ActionResult<IEnumerable<EventWithParticipantsDto>>> GetEventParticipants(int id)
    {
        var participants = await _getParticipantsByEventIdUseCase.ExecuteAsync(id);
        return Ok(participants);
    }

    [HttpGet("user-{id:int}")]
    public async Task<ActionResult<IEnumerable<UserWithParticipantsDto>>> GetUserParticipants(int id)
    {
        var participants = await _getParticipantsByUserIdUseCase.ExecuteAsync(id);
        return Ok(participants);
    }

    [HttpPost("{eventId:int}/to/{userId:int}")]
    [Authorize]
    public async Task<IActionResult> PostEventParticipant(int eventId, int userId)
    {
        await _updatePaticipationUseCase.ExecuteAsync(eventId, userId);
        return NoContent();
    }

    [HttpDelete("{eventId:int}/from/{userId:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteEventParticipant(int eventId, int userId)
    {
        await _removeParticipationUseCase.ExecuteAsync(eventId, userId);
        return NoContent();
    }
}
