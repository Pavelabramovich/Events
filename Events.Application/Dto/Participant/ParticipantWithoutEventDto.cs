
namespace Events.Application.Dto;


public record ParticipantWithoutEventDto
{
    public required int UserId { get; init; }
    public required string UserName { get; init; }
    public required string UserSurname { get; init; }

    public required DateTime RegistrationTime { get; init; }
}
