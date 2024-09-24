
namespace Events.Domain.Entities;


public record User : Entity
{
    public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public DateOnly DateOfBirth { get; init; } = default!;

    public string Login { get; init; } = default!;
    public string HashedPassword { get; init; } = default!;
    public string SecurityStamp { get; init; } = default!;

    public ICollection<Claim> Claims { get; init; } = new List<Claim>();
    public ICollection<ExternalLogin> ExternalLogins { get; init; } = new List<ExternalLogin>();
    public ICollection<Role> Roles { get; init; } = new List<Role>();

    public IEnumerable<Event> Events { get; init; } = new List<Event>();
    public ICollection<Participation> Participants { get; init; } = new List<Participation>();
}
