using Microsoft.EntityFrameworkCore;
using Events.Entities;


namespace Events.WebApi.Db;


public static class DataInitializer
{
    public static void Seed(EventsContext context)
    {
        if (!context.Events.Any())
        {
            context.Events.AddRange(
                new Event() { Name = "Name1", Address = "A", Description = "D", ImagePath = "I" },
                new Event() { Name = "Name2", Address = "A", Description = "D", ImagePath = "I" },
                new Event() { Name = "Name3", Address = "A", Description = "D", ImagePath = "I" }
            );

            context.Users.AddRange(
                new User() { Name = "User1", Email = "E1", Password = "P", Surname = "S" },
                new User() { Name = "User2", Email = "E2", Password = "P", Surname = "S" },
                new User() { Name = "User3", Email = "E3", Password = "P", Surname = "S" }
            );

            context.SaveChanges(); 
        }
    }
}