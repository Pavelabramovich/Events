using Microsoft.EntityFrameworkCore;
using Events.Entities;


namespace Events.Models;


public class EventsContext : DbContext
{
    public EventsContext(DbContextOptions<EventsContext> options)
        : base(options)
    { }

    public DbSet<Event> Events { get; } = default!;
    public DbSet<User> Users { get; } = default!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureEvent(modelBuilder);
        ConfigureUser(modelBuilder);
    }


    private void ConfigureEvent(ModelBuilder modelBuilder)
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

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasKey(e => e.Id);
    }
}