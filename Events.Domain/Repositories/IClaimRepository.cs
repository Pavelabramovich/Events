using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IClaimRepository : IRepository<Claim>
{
    IEnumerable<Claim> GetUserClaims(int userId);
    IAsyncEnumerable<Claim> GetUserClaimsAsync(int userId, CancellationToken cancellationToken = default);
}