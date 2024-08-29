using Microsoft.EntityFrameworkCore;
using Events.Entities;


namespace Events.WebApi.Db;


public class EventsContext : DbContext
{
    public EventsContext(DbContextOptions<EventsContext> options)
        : base(options)
    {
        DataInitializer.Seed(this);
    }

    public DbSet<Event> Events { get; private set; } = default!;
    public DbSet<User> Users { get; private set; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureEvent(modelBuilder);
        ConfigureUser(modelBuilder);
    }


    private static void ConfigureEvent(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Event>()
            .HasKey(e => e.Id);

        modelBuilder
           .Entity<Event>()
           .HasMany<User>(e => e.Participants)
           .WithOne()
           .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasKey(e => e.Id);
    }
}