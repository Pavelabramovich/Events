using Events.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Events.DataBase.Configuration;


internal class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
{
    public void Configure(EntityTypeBuilder<ExternalLogin> builder)
    {
        builder.ToTable("external_logins");

        builder
            .HasKey(l => new { l.Provider, l.ProviderKey, l.UserId });

        builder
            .Property(l => l.Provider)
            .HasColumnName("provider")
            .HasMaxLength(128)
            .IsRequired();

        builder
            .Property(l => l.ProviderKey)
            .HasColumnName("provider_key")
            .HasMaxLength(128)
            .IsRequired();

        builder
           .HasOne(l => l.User)
           .WithMany(u => u.ExternalLogins)
           .HasForeignKey(c => c.UserId)
           .IsRequired();

        builder
            .Property(l => l.UserId)
            .HasColumnName("user_id");
    }
}
