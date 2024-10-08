﻿using Microsoft.AspNetCore.Mvc;
using Events.Domain;
using Events.Application.Dto;
using Events.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Events.Application.Exceptions;
using FluentValidation;
using Events.Domain.Entities;


namespace Events.WebApi.Controllers;


[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly GetAllEventsUseCase _getAllUseCase;
    private readonly GetEventByIdUseCase _getByIdUseCase;
    private readonly GetEventByNameUseCase _getByNameUseCase;
    private readonly GetEventsPageUseCase _getPageUseCase;
    private readonly CreateEventUseCase _createUseCase;
    private readonly UpdateEventUseCase _updateUseCase;
    
    private readonly RemoveEventUseCase _removeUseCase;


    public EventsController(
        GetAllEventsUseCase getAllUseCase,
        GetEventByIdUseCase getByIdUseCase,
        GetEventByNameUseCase getByNameUseCase,
        GetEventsPageUseCase getPageUseCase,
        CreateEventUseCase createUseCase,
        UpdateEventUseCase updateUseCase,
        RemoveEventUseCase removeUseCase)
    {
        _getAllUseCase = getAllUseCase;
        _getByIdUseCase = getByIdUseCase;
        _getByNameUseCase = getByNameUseCase;

        _getPageUseCase = getPageUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        
        _removeUseCase = removeUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventWithoutParticipantsDto>>> GetEvents()
    {
        var events = await _getAllUseCase.ExecuteAsync();
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventWithoutParticipantsDto>> GetEvent(int id)
    {
        return await _getByIdUseCase.ExecuteAsync(id);
    }

    [HttpGet("page/{pageNum:int}-of-{pageSize:int}")]
    public async Task<ActionResult<IEnumerable<EventWithoutParticipantsDto>>> GetEventsPage(int pageNum, int pageSize)
    {
        var events = await _getPageUseCase.ExecuteAsync(pageNum, pageSize);
        return Ok(events);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Event>> PostEvent(EventCreatingDto eventDto)
    {
        await _createUseCase.ExecuteAsync(eventDto);
        var newEvent = await _getByNameUseCase.ExecuteAsync(eventDto.Name);

        return CreatedAtAction(nameof(GetEvent), new { id = newEvent!.Id }, newEvent);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutEvent(EventWithoutParticipantsDto eventDto)
    {
        await _updateUseCase.ExecuteAsync(eventDto); 
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        await _removeUseCase.ExecuteAsync(id);
        return NoContent();
    }
}
