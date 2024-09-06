using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities;
using Events.DataBase.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace Events.DataBase.Configuration;


internal class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.ToTable("claims");

        builder
            .HasKeyProperty(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder
            .HasOne(c => c.User)
            .WithMany(u => u.Claims)
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(c => c.UserId)
            .HasColumnName("user_id");

        builder
            .Property(c => c.Type)
            .HasColumnName("type")
            .IsOptional();

        builder
            .Property(c => c.Value)
            .HasColumnName("value")
            .IsOptional();
    }
}