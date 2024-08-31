
using Events.Entities;

namespace Events.WebApi.Dto;


public record EventToAddDto
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTime DateTime { get; init; }
    public required string Address { get; init; }
    public required Category Category { get; init; }
    public required int MaxPeopleCount { get; init; }
    public required string ImagePath { get; init; }
}


public record EventWithParticipantsDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } 
    public required string Description { get; init; } 
    public required DateTime DateTime { get; init; } 
    public required string Address { get; init; } 
    public required Category Category { get; init; } 
    public required int MaxPeopleCount { get; init; }
    public required string ImagePath { get; init; }

    public required ICollection<ParticipantWithoutEventDto> Participants { get; init; }
}


public record EventWithoutParticipantsDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTime DateTime { get; init; }
    public required string Address { get; init; }
    public required Category Category { get; init; }
    public required int MaxPeopleCount { get; init; }
    public required string ImagePath { get; init; }
}
