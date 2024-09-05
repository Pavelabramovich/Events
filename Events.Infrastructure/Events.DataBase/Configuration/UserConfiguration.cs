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
            .IsOptional();

        builder
            .Property(u => u.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsOptional();

        builder
            .Property(u => u.Login)
            .HasColumnName("login")
            .IsRequired();

        builder
            .Property(u => u.HashedPassword)
            .HasColumnName("hashed_password")
            .IsRequired();
    
        builder
            .Property(u => u.SecurityStamp)
            .HasColumnName("security_stamp")
            .IsOptional();

        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users);

        builder
            .HasMany(u => u.Claims)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        builder
            .HasMany(u => u.ExternalLogins)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);

        builder
            .HasMany(u => u.Events)
            .WithMany(e => e.Users)
            .UsingEntity<Participation>(
                r => r.HasOne(p => p.Event).WithMany(e => e.Participants).HasForeignKey(p => p.EventId),
                l => l.HasOne(p => p.User).WithMany(u => u.Participants).HasForeignKey(p => p.UserId),
                j => j.HasKey(p => p.Id)
            );
    }
}
