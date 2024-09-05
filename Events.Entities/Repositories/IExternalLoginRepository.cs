using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IExternalLoginRepository : IRepository<ExternalLogin>
{
    ExternalLogin? GetByProviderAndKey(string loginProvider, string providerKey);
    Task<ExternalLogin?> GetByProviderAndKeyAsync(string loginProvider, string providerKey);
    Task<ExternalLogin?> GetByProviderAndKeyAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
}
