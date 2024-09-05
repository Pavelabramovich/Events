using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.DataBase.Repositories;


internal class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
{
    internal ExternalLoginRepository(EventsContext context)
        : base(context)
    { }


    public ExternalLogin? GetByProviderAndKey(string loginProvider, string providerKey)
    {
        return Set.FirstOrDefault(l => l.Provider == loginProvider && l.ProviderKey == providerKey);
    }

    public Task<ExternalLogin?> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
    {
        return Set.FirstOrDefaultAsync(l => l.Provider == loginProvider && l.ProviderKey == providerKey);
    }

    public Task<ExternalLogin?> GetByProviderAndKeyAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        return Set.FirstOrDefaultAsync(l => l.Provider == loginProvider && l.ProviderKey == providerKey, cancellationToken);
    }
}
