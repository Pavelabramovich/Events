
namespace Events.Domain.Entities;


public record ExternalLogin : IEntity
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string Provider { get; init; } = default!;
    public string ProviderKey { get; init; } = default!;

    object IEntity.Id => new { UserId, Provider, ProviderKey };
}
