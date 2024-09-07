using Events.Domain;
using Events.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace Events.DataBase.Repositories;


internal class ClaimRepository : Repository<Claim>, IClaimRepository
{
    internal ClaimRepository(EventsContext context)
        : base(context)
    { }


    public IEnumerable<Claim> GetUserClaims(int userId)
    {
        return Set.AsNoTracking().Where(r => r.UserId == userId).ToArray();
    }

    public async IAsyncEnumerable<Claim> GetUserClaimsAsync(int userId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var claim in Set.AsNoTracking().Where(r => r.UserId == userId).AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return claim;
        }
    }
}
