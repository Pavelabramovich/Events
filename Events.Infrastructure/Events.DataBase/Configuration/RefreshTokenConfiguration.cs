using Events.DataBase.Extensions;
using Events.WebApi.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Events.DataBase.Configuration;


internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .HasKeyProperty(t => t.UserId)
            .ValueGeneratedNever();

        builder
            .HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<RefreshToken>(t => t.UserId);
    }
}
