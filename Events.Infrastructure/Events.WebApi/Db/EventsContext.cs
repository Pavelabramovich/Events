using Microsoft.EntityFrameworkCore;
using Events.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Events.WebApi.Db;


public class EventsContext : DbContext
{
    public EventsContext(DbContextOptions<EventsContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<Event> Events { get; private set; } 

    public DbSet<User> Users { get; private set; }
    public DbSet<IdentityUserClaim<int>> UserClaims { get; private set; }


  //  public DbSet<Role> Roles { get; private set; }

    public DbSet<RefreshToken> UserRefreshToken { get; private set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureEvent(modelBuilder);
        ConfigureUser(modelBuilder);

        SeedDefaultData(modelBuilder);
    }


    private static void ConfigureEvent(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Event>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Event>()
            .HasMany(e => e.Users)
            .WithMany(u => u.Events)
            .UsingEntity<Participation>(
                l => l.HasOne(p => p.User).WithMany(u => u.Participants).HasForeignKey(p => p.UserId),
                r => r.HasOne(p => p.Event).WithMany(e => e.Participants).HasForeignKey(p => p.EventId),
                j => j.HasKey(p => p.Id)
            );
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasKey(e => e.Id);

        modelBuilder
            .Entity<Participation>()
            .HasKey(p => p.Id);


        modelBuilder
            .Entity<IdentityUserClaim<int>>()
            .HasKey(c => c.Id);

        modelBuilder
            .Entity<IdentityUserClaim<int>>()
            .ToTable("UserClaims");


        modelBuilder
            .Entity<RefreshToken>()
            .HasKey(t => t.UserId);

        modelBuilder 
            .Entity<RefreshToken>()
            .Property(t => t.UserId)
            .IsRequired()
            .ValueGeneratedNever();

        modelBuilder
            .Entity<RefreshToken>()
            .HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<RefreshToken>(t => t.UserId);
           // .OnDelete(DeleteBehavior.Cascade);

        //modelBuilder
        //    .Entity<Role>()
        //    .HasKey(r => r.Id);

        //modelBuilder
        //    .Entity<Role>()
        //    .HasMany(r => r.Users)
        //    .WithMany(u => u.Roles)
        //    .UsingEntity(j => j.HasData(
        //    [
        //        new { UserId = 1, RoleId = 1 },
        //        new { UserId = 2, RoleId = 2 },
        //        new { UserId = 1, RoleId = 1 },
        //    ]));
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
            new() { Id = 1, UserName = "Pasha", Email = "lol@gmail.com", Password = "Pass123", Surname = "First" },
            new() { Id = 2, UserName = "Petia", Email = "crol@mail.ru", Password = "Vass123", Surname = "Second" },
            new() { Id = 3, UserName = "Vova", Email = "esc@gmama.help", Password = "Kiss123", Surname = "Third" }
        ]);

        modelBuilder.Entity<IdentityUserClaim<int>>().HasData(new IdentityUserClaim<int>
        {
            Id = 1,
            UserId = 1,
            ClaimType = ClaimTypes.Role,
            ClaimValue = "Admin"
        });

        modelBuilder.Entity<Participation>().HasData(
        [
            new() { Id = 1, EventId = 1, UserId = 1 },
            new() { Id = 2, EventId = 1, UserId = 2 },

            new() { Id = 3, EventId = 2, UserId = 3 }
        ]);
    }
}