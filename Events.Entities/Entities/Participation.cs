using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public record Participation : Entity
{
    public int EventId { get; init; } = default!;
    public Event Event { get; init; } = default!;

    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public DateTime RegistrationTime { get; init; } = default!;
}
