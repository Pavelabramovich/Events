using Events.Domain.Entities;


namespace Events.Entities;


public interface IExternalLoginRepository : IRepository<ExternalLogin, object>
{
    ExternalLogin GetByProviderAndKey(string loginProvider, string providerKey);
    Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey);
    Task<ExternalLogin> GetByProviderAndKeyAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
}
