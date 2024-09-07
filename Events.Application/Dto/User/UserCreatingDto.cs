
namespace Events.Application.Dto;


public record UserCreatingDto
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string Login { get; init; }
    public required string HashedPassword { get; init; }
}
