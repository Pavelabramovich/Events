using Events.DataBase.Extensions;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Events.DataBase.Configuration;


internal class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("events");

        builder
            .HasKeyProperty(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(32)
            .IsOptional();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();

        builder
            .Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(1024)
            .IsOptional();

        builder
            .Property(e => e.DateTime)
            .HasColumnName("date_time")
            .IsRequired();

        builder
            .Property(e => e.Address)
            .HasColumnName("address")
            .HasMaxLength(128)
            .IsOptional();

        builder
            .Property(e => e.Category)
            .HasColumnName("category")
            .IsRequired();

        builder
            .Property(e => e.MaxPeopleCount)
            .HasColumnName("max_people_count")
            .IsRequired();

        builder
            .Property(e => e.ImagePath)
            .HasColumnName("image")
            .IsOptional();

        builder
            .HasMany(e => e.Users)
            .WithMany(u => u.Events)
            .UsingEntity<Participation>(
                r => r.HasOne(p => p.User).WithMany(u => u.Participants).HasForeignKey(p => p.UserId),
                l => l.HasOne(p => p.Event).WithMany(e => e.Participants).HasForeignKey(p => p.EventId),
                j =>
                {
                    j.ToTable("participations");
                    j.HasKey(p => p.Id);
                    j.Property(p => p.EventId).HasColumnName("event_id");
                    j.Property(p => p.UserId).HasColumnName("user_id");
                    j.Property(p => p.RegistrationTime).HasColumnName("registration_time");
                }
            );
    }
}
