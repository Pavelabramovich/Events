
namespace Events.WebApi.Dto;


public record ParticipantWithoutEventDto
{
    public required int UserId { get; init; }
    public required string UserName { get; init; }
    public required string UserSurname { get; init; }

    public required DateTime RegistrationTime { get; init; }
}


public record ParticipantWithoutUserDto
{
    public required int EventId { get; init; }
    public required string EventName { get; init; }
    public required string EventDescription { get; init; }

    public required DateTime RegistrationTime { get; init; }
}
