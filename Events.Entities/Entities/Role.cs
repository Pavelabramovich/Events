using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public record Role : UniqueNameEntity
{
    public ICollection<User> Users { get; init; } = new List<User>();
}
