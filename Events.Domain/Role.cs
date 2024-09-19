
namespace Events.Domain;


public record Role : UniqueNameEntity
{
    public ICollection<User> Users { get; init; } = new List<User>();
}
