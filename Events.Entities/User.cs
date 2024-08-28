using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public class User : Entity
{
    public string Name { get; init; }
    public string Surname { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}
