using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Domain.Repositories;


public interface IClaimRepository : IRepository<Claim>
{
    IEnumerable<Claim> GetUserClaims(int userId);
    IAsyncEnumerable<Claim> GetUserClaimsAsync(int userId, CancellationToken cancellationToken = default);
}