
namespace Events.WebApi.Dto;

public record ParticipantWithoutUserDto
{
    public required int EventId { get; init; }
    public required string EventName { get; init; }
    public required string EventDescription { get; init; }

    public required DateTime RegistrationTime { get; init; }
}
