using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Events.DataBase.Repositories;


internal class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
{
    internal ExternalLoginRepository(EventsContext context)
        : base(context)
    { }


    public ExternalLogin? GetByProviderAndKey(string loginProvider, string providerKey)
    {
        return Set.AsNoTracking().FirstOrDefault(l => l.Provider == loginProvider && l.ProviderKey == providerKey);
    }

    public Task<ExternalLogin?> GetByProviderAndKeyAsync(string loginProvider, string providerKey)
    {
        return Set.AsNoTracking().FirstOrDefaultAsync(l => l.Provider == loginProvider && l.ProviderKey == providerKey);
    }

    public Task<ExternalLogin?> GetByProviderAndKeyAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        return Set.AsNoTracking().FirstOrDefaultAsync(l => l.Provider == loginProvider && l.ProviderKey == providerKey, cancellationToken);
    }
}
