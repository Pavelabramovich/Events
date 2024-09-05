using Events.DataBase.Extensions;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.DataBase.Configuration;


internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder
            .HasKeyProperty(r => r.Name)
            .HasColumnName("name")
            .IsRequired();

        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
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


        /*
         
         .UsingEntity<Dictionary<string, object>>(
            "user_roles", // Название промежуточной таблицы
            j => j.HasOne<Role>()
                  .WithMany()
                  .HasForeignKey("RoleId"),
            j => j.HasOne<User>()
                  .WithMany()
                  .HasForeignKey("UserId"),
            j =>
            {
                j.HasKey("UserId", "RoleId"); // Композитный ключ
                j.Property("UserId").HasColumnName("user_id");
                j.Property("RoleId").HasColumnName("role_id");
            }
        );
         
         */
    }
}
