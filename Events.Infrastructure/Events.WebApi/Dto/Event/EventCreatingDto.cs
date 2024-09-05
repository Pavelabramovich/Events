using Events.Domain_Entities;


namespace Events.WebApi.Dto;


public record EventCreatingDto
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTime DateTime { get; init; }
    public required string Address { get; init; }
    public required Category Category { get; init; }
    public required int MaxPeopleCount { get; init; }
    public required string ImagePath { get; init; }
}
