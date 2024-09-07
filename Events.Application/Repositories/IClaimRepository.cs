using Events.Domain;


namespace Events.Application.Repositories;


public interface IClaimRepository : IRepository<Claim>
{
    IEnumerable<Claim> GetUserClaims(int userId);
    IAsyncEnumerable<Claim> GetUserClaimsAsync(int userId, CancellationToken cancellationToken = default);
}