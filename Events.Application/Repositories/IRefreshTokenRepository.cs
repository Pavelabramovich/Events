using Events.Domain;


namespace Events.Application.Repositories;


public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    RefreshToken? GetSavedRefreshToken(int userId, string refreshToken);
    Task<RefreshToken?> GetSavedRefreshTokedAsync(int userId, string refreshToken, CancellationToken cancellationToken = default);

    void UpsertUserRefreshToken(RefreshToken refreshToken);
    void DeleteUserRefreshToken(int userId, string refreshToken);
}
