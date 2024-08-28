
namespace Events.Entities;


public class Event : Entity
{
    public string Neme { get; init; }
    public string Description { get; init; }
    public DateTime DateTime { get; init; }
    public string Address { get; init; }
    public Category Category { get; init; }
    public int MaxPeopleCount { get; init; }
    public IEnumerable<User> Participants { get; init; }
    public string ImagePath { get; init; }
}
