
namespace Events.WebApi.Dto;


public class UserDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } 
    public required string Surname { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
