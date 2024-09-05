using Events.Entities;


namespace Events.Domain.Entities;


public record Claim : Entity
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string ClaimType { get; init; } = default!;
    public string ClaimValue { get; init; } = default!;
}
