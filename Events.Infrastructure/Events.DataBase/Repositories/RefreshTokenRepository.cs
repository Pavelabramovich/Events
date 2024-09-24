using Events.Domain.Repositories;
using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Events.DataBase.Repositories;


internal class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(EventsContext context)
        : base(context)
    { }


    public RefreshToken? GetSavedRefreshToken(int userId, string refreshToken)
    {
        return Set.AsNoTracking().FirstOrDefault(t => t.UserId == userId && t.Value == refreshToken);
    }

    public Task<RefreshToken?> GetSavedRefreshTokedAsync(int userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        return Set.AsNoTracking().FirstOrDefaultAsync(t => t.UserId == userId && t.Value == refreshToken, cancellationToken);
    }


    public void UpsertUserRefreshToken(RefreshToken refreshToken)
    {
        Set.Upsert(refreshToken).Run();
    }

    public void DeleteUserRefreshToken(int userId, string refreshToken)
    {
        var token = Set.AsNoTracking().FirstOrDefault(t => t.UserId == userId && t.Value == refreshToken);

        if (token is not null)
            Set.Remove(token);
    }
}
