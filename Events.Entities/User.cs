using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public record User(
    string Name,
    string Surname,
    DateOnly DateOfBirth,
    string Email
);
