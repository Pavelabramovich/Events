using Events.DataBase.Extensions;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace Events.DataBase.Configuration;


internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder
            .HasKeyProperty(u => u.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder
            .Property(u => u.Name)
            .HasColumnName("name")
             .HasMaxLength(32)
            .IsOptional();

        builder
            .Property(u => u.Surname)
            .HasColumnName("surname")
            .HasMaxLength(32)
            .IsOptional();

        builder
            .Property(u => u.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder
            .Property(u => u.Login)
            .HasColumnName("login")
            .HasMaxLength(32)
            .IsRequired();

        builder
            .HasIndex(u => u.Login)
            .IsUnique();

        builder
            .Property(u => u.HashedPassword)
            .HasColumnName("hashed_password")
            .HasMaxLength(32)
            .IsRequired();
    
        builder
            .Property(u => u.SecurityStamp)
            .HasColumnName("security_stamp")
            .IsOptional();

        builder
            .HasMany(u => u.ExternalLogins)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);

        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(
                "UserRole",
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId"),
                l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleName"),
                j =>
                {
                    j.ToTable("user_roles");
                    j.Property("RoleName").HasColumnName("role_name");
                    j.Property("UserId").HasColumnName("user_id");
                }
            );

        builder
            .HasMany(u => u.Claims)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.Events)
            .WithMany(e => e.Users)
            .UsingEntity<Participation>(
                r => r.HasOne(p => p.Event).WithMany(e => e.Participants).HasForeignKey(p => p.EventId),
                l => l.HasOne(p => p.User).WithMany(u => u.Participants).HasForeignKey(p => p.UserId),
                j =>
                {
                    j.ToTable("participations");
                    j.HasKey(p => p.Id);
                    j.Property(p => p.UserId).HasColumnName("user_id");
                    j.Property(p => p.EventId).HasColumnName("event_id");
                    j.Property(p => p.RegistrationTime).HasColumnName("registration_time");
                }
            );
    }
}
