using Events.Domain.Entities;


namespace Events.Application.Dto;


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

    public required IEnumerable<ParticipantWithoutEventDto> Participants { get; init; }
}
