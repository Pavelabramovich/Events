using Microsoft.AspNetCore.Identity;


namespace Events.Entities;


public class User : IdentityUser<int>
{
  //  public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public DateOnly DateOfBirth { get; init; } = default!;
  //  public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;

    public IEnumerable<Event> Events { get; init; } = new List<Event>();
    public ICollection<Participation> Participants { get; init; } = new List<Participation>();

  //  public ICollection<Role> Roles { get; init; } = new List<Role>();
}
