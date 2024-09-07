
namespace Events.Application.Dto;


public record UserWithParticipantsDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required DateOnly DateOfBirth { get; init; }

    public required IEnumerable<ParticipantWithoutUserDto> Events { get; init; }
}
