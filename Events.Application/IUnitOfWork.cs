using Events.Application.Repositories;


namespace Events.Application;


public interface IUnitOfWork : IDisposable
{
    IEventRepository EventRepository { get; }
    IUserRepository UserRepository { get; }
    IExternalLoginRepository ExternalLoginRepository { get; }
    IRoleRepository RoleRepository { get; }
    IClaimRepository ClaimRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }

    bool SaveChanges();
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
