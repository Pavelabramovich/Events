using Microsoft.EntityFrameworkCore;
using Events.Entities;
using Microsoft.Extensions.Hosting;


namespace Events.WebApi.Db;


public class EventsContext : DbContext
{
    public EventsContext(DbContextOptions<EventsContext> options)
        : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;

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

        modelBuilder.Entity<Event>()  // post = event, tag = user
            .HasMany(e => e.Users)
            .WithMany(u => u.Events)
            .UsingEntity<Participation>(
                l => l.HasOne<User>().WithMany(u => u.Participants).HasForeignKey(p => p.UserId).HasPrincipalKey(u => u.Id),
                r => r.HasOne<Event>().WithMany(e => e.Participants).HasForeignKey(p => p.EventId).HasPrincipalKey(e => e.Id),
                j => j.HasKey(p => new { p.UserId, p.EventId })
            );

        //modelBuilder.Entity<Event>()
        //    .HasMany(e => e.Participants)
        //    .WithMany(e => e.Events);

        //modelBuilder
        //   .Entity<Event>()
        //   .HasMany<User>(e => e.Participants)
        //   .WithOne()
        //   .OnDelete(DeleteBehavior.NoAction);
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasKey(e => e.Id);

        modelBuilder
            .Entity<Participation>()
            
            .HasKey(p => p.Id);
    }
}