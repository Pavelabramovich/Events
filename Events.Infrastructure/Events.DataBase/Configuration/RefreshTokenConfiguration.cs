using Events.DataBase.Extensions;
using Events.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Events.DataBase.Configuration;


internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder
            .HasKeyProperty(t => t.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder
            .HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<RefreshToken>(t => t.UserId);

        builder
            .Property(t => t.Value)
            .HasColumnName("value")
            .IsRequired();

        builder
            .Property(t => t.Expires)
            .HasColumnName("expires")
            .IsRequired();
    }
}
