
namespace Events.Domain.Entities;


public interface IEntity
{
    object Id { get; }
}


public interface IEntity<out TKey> : IEntity
{
    new TKey Id { get; }
}

public record Entity<TKey> : IEntity<TKey>, IEntity
{
    public TKey Id { get; init; } = default!;

    object IEntity.Id => Id!;
}


public record Entity : Entity<int>;

public record GuidEntity : Entity<Guid>;

public record UniqueNameEntity : IEntity<string>, IEntity
{
    public string Name { get; init; } = default!;

    string IEntity<string>.Id => Name;
    object IEntity.Id => Name;
}
