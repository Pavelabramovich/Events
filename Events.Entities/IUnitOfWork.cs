using Events.Domain.Repositories;


namespace Events.Domain;


public interface IUnitOfWork : IDisposable
{
    IEventRepository EventRepository { get; }

    IUserRepository UserRepository { get; }
    IExternalLoginRepository ExternalLoginRepository { get; }
    IRoleRepository RoleRepository { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
