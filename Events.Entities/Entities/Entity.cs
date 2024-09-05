using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public interface IEntity<out TKey>
{
    TKey Id { get; }
}



public record Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; init; } = default!;
}

public record Entity : Entity<int>;

public record GuidEntity : Entity<Guid>;

public record UniqueNameEntity : IEntity<string>
{
    public string Name { get; init; } = default!;

    string IEntity<string>.Id => Name;
}
