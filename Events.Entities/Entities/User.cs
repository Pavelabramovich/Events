using Events.Domain.Entities;


namespace Events.Entities;


public record User : Entity
{
    public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;

    public string HashedPassword { get; init; } = default!;
    public string SecurityStamp { get; init; } = default!;

    public ICollection<Claim> Claims { get; init; } = new List<Claim>();
    public ICollection<ExternalLogin> Logins { get; init; } = new List<ExternalLogin>();
    public ICollection<Role> Roles { get; init; } = new List<Role>();  
}
