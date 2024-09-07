using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Events.DataBase.Configuration;
using Events.Domain;
using Events.WebApi.Authentication;


namespace Events.DataBase;


internal class EventsContext : DbContext
{
    public EventsContext()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public EventsContext(DbContextOptions<EventsContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }


    public DbSet<Event> Events { get; private set; }

    public DbSet<User> Users { get; private set; }
    public DbSet<ExternalLogin> ExternalLogins { get; private set; }
    public DbSet<Role> Roles { get; private set; }
    public DbSet<Claim> Claims { get; private set; }

    public DbSet<RefreshToken> RefreshTokens { get; private set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new EventConfiguration().Configure(modelBuilder.Entity<Event>());

        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new ExternalLoginConfiguration().Configure(modelBuilder.Entity<ExternalLogin>());
        new RoleConfiguration().Configure(modelBuilder.Entity<Role>());
        new ClaimConfiguration().Configure(modelBuilder.Entity<Claim>());

        new RefreshTokenConfiguration().Configure(modelBuilder.Entity<RefreshToken>());


        SeedDefaultData(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;User Id=postgres;Password=NotSqlite;Database=events;Include Error Detail = true;");
        }
    }


    private static void SeedDefaultData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>().HasData(
        [
            new()
            {
                Id = 1,
                Name = "Concert",
                Category = Category.Concert,
                DateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                MaxPeopleCount = 4,
                Address = "Minsk 123",
                Description = "Top level concert",
                ImagePath = "concert.png",
            },
            new()
            {
                Id = 2,
                Name = "Allowed meeting",
                Category = Category.Meeting,
                DateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                MaxPeopleCount = 10,
                Address = "Mos cow, 12",
                Description = "description ...",
                ImagePath = "meeting.png",
            },
            new()
            {
                Id = 3,
                Name = "Fair with tail",
                Category = Category.Fair,
                DateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                Address = "Paris, Sena",
                MaxPeopleCount = 9,
                Description = "Frogs?",
                ImagePath = "paris.jpg",
            }
        ]);

        modelBuilder.Entity<User>().HasData(
        [
            new() { Id = 1, Name = "Pasha", Login = "lol@gmail.com", HashedPassword = "Pass123", Surname = "First" },
            new() { Id = 2, Name = "Petia", Login = "crol@mail.ru", HashedPassword = "Vass123", Surname = "Second" },
            new() { Id = 3, Name = "Vova", Login = "esc@gmama.help", HashedPassword = "Kiss123", Surname = "Third" }
        ]);

        modelBuilder.Entity<Role>().HasData(
        [
            new() { Name = "Admin" }    
        ]);


        modelBuilder.Entity("UserRole").HasData(
        [
            new { UserId = 1, RoleName = "Admin" }    
        ]);

        modelBuilder.Entity<Participation>().HasData(
        [
            new() { Id = 1, EventId = 1, UserId = 1, RegistrationTime = DateTime.UtcNow },
            new() { Id = 2, EventId = 1, UserId = 2, RegistrationTime = DateTime.UtcNow },

            new() { Id = 3, EventId = 2, UserId = 3, RegistrationTime = DateTime.UtcNow }
        ]);
    }
}
