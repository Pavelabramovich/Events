
namespace Events.Domain;


public record Event : Entity
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public DateTime DateTime { get; init; } = default!;
    public string Address { get; init; } = default!;
    public Category Category { get; init; } = default!;
    public int MaxPeopleCount { get; init; } = default!;
    public string ImagePath { get; init; } = default!;

    public IEnumerable<User> Users { get; init; } = new List<User>();
    public ICollection<Participation> Participants { get; init; } = new List<Participation>();
}
