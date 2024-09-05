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
            .WithMany(u => u.Roles);
    }
}
