
namespace Events.Domain.Entities;


public record Claim : Entity
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string Type { get; init; } = default!;
    public string Value { get; init; } = default!;
}
