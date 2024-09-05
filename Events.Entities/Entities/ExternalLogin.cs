using Events.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Domain.Entities;


public record ExternalLogin : IEntity<object>
{
    public int UserId { get; init; } = default!;
    public User User { get; init; } = default!;

    public string LoginProvider { get; init; } = default!;
    public string ProviderKey { get; init; } = default!;

    object IEntity<object>.Id => new { UserId, LoginProvider, ProviderKey };
}