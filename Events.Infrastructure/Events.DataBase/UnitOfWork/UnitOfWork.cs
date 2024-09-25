using Events.DataBase.Repositories;
using Events.Domain.Repositories;
using Events.Domain;
using Microsoft.EntityFrameworkCore;


namespace Events.DataBase;


public class UnitOfWork : IUnitOfWork
{
    private protected readonly EventsContext _context;

    private readonly Lazy<IEventRepository> _eventRepositoryLazy;
    private readonly Lazy<IUserRepository> _userRepositoryLazy;
    private readonly Lazy<IExternalLoginRepository> _externalLoginRepositoryLazy;
    private readonly Lazy<IRoleRepository> _roleRepositoryLazy;
    private readonly Lazy<IClaimRepository> _claimRepositoryLazy;
    private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepositoryLazy;

    protected bool _disposed;


    public UnitOfWork()
    {
        _context = new EventsContext();

        _eventRepositoryLazy = new(() => new EventRepository(_context));
        _userRepositoryLazy = new(() => new UserRepository(_context));
        _externalLoginRepositoryLazy = new(() => new ExternalLoginRepository(_context));
        _roleRepositoryLazy = new(() => new RoleRepository(_context));
        _claimRepositoryLazy = new(() => new ClaimRepository(_context));
        _refreshTokenRepositoryLazy = new(() => new RefreshTokenRepository(_context));
    }


    public IEventRepository EventRepository
    {
        get => !_disposed
            ? _eventRepositoryLazy.Value
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed.");
    }

    public IUserRepository UserRepository
    {
        get => !_disposed
            ? _userRepositoryLazy.Value
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed.");
    }

    public IExternalLoginRepository ExternalLoginRepository
    {
        get  => !_disposed
            ? _externalLoginRepositoryLazy.Value
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed."); 
    }

    public IRoleRepository RoleRepository
    {
        get => !_disposed
            ? _roleRepositoryLazy.Value
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed."); 
    }

    public IClaimRepository ClaimRepository
    {
        get => !_disposed
            ? _claimRepositoryLazy.Value
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed.");
    }

    public IRefreshTokenRepository RefreshTokenRepository
    {
        get => !_disposed
            ? _refreshTokenRepositoryLazy.Value
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed.");
    }


    public void SaveChanges()
    {
        _context.SaveChanges(); 
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }


    public void Dispose()
    {
        if (_disposed)
            return;

        _context.Dispose();
        
        _disposed = true;
    }
}
