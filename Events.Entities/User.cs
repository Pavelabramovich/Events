using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public record User : Entity
{
    public string Name { get; init; } = default!;
    public string Surname { get; init; } = default!;
    public DateOnly DateOfBirth { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;

    public IEnumerable<Event> Events { get; init; } = new List<Event>();
    public ICollection<Participation> Participants { get; init; } = new List<Participation>();
}
