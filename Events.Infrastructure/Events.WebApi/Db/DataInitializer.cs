using Microsoft.EntityFrameworkCore;
using Events.Entities;
using NuGet.Packaging;


namespace Events.WebApi.Db;


public static class DataInitializer
{
    public static void Seed(EventsContext context)
    {
        if (!context.Events.Any())
        {
            var u1 = new User() { Id = 1, Name = "Pasha", Email = "lol@gmail.com", Password = "Pass123", Surname = "First" };
            var u2 = new User() { Id = 2, Name = "Petia", Email = "crol@mail.ru", Password = "Vass123", Surname = "Second" };
            var u3 = new User() { Id = 3, Name = "Vova", Email = "esc@gmama.help", Password = "Kiss123", Surname = "Third" };

            var e1 = new Event() 
            { 
                Id = 1,
                Name = "Concert", 
                Category = Category.Concert, 
                DateTime = DateTime.Now,
                MaxPeopleCount = 4, 
                Address = "Minsk 123", 
                Description = "Top level concert", 
                ImagePath = "concert.png",
            };

            var e2 = new Event() 
            { 
                Id = 2,
                Name = "Allowed meeting", 
                Category = Category.Meeting,
                DateTime = DateTime.Now - TimeSpan.FromDays(1),
                MaxPeopleCount = 10,
                Address = "Mos cow, 12", 
                Description = "description ...", 
                ImagePath = "meeting.png",
            };

            var e3 = new Event() 
            { 
                Id = 3,
                Name = "Fair with tail", 
                Category = Category.Fair,
                DateTime = DateTime.Now - TimeSpan.FromDays(7),
                Address = "Paris, Sena", 
                MaxPeopleCount = 9,
                Description = "Frogs?", 
                ImagePath = "paris.jpg", 
            };

            e1.Participants.AddRange(
            [
                new() { EventId = e1.Id, Event = e1, UserId = u1.Id, User = u1 }, 
                new() { EventId = e1.Id, Event = e1, UserId = u2.Id, User = u2 }
            ]);

            e2.Participants.AddRange(
            [
                new() { EventId = e2.Id, Event = e2, UserId = u3.Id, User = u3 }    
            ]);

            context.Events.AddRange(e1, e2, e3);
            context.Users.AddRange(u1, u2, u3);

            context.SaveChanges(); 
        }
    }
}