
using System.ComponentModel.DataAnnotations;

namespace Events.Entities;


public record Event(
    string Name,
    string Description,
    DateTime DateTime,
    string Address,
    Category Category,
    int MaxPeopleCount,
    IEnumerable<User> Participants,
    string ImagePath
);
