using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Events.DataBase.Configuration;
using Events.Domain.Entities;
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
    }
}
