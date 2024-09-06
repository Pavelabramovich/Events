using Events.DataBase.Repositories;
using Events.Domain.Repositories;
using Events.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading;


namespace Events.DataBase;


public class UnitOfWork : IUnitOfWork
{
    private protected readonly EventsContext _context;

    private readonly Lazy<IEventRepository> _eventRepositoryLazy;
    private readonly Lazy<IUserRepository> _userRepositoryLazy;
    private readonly Lazy<IExternalLoginRepository> _externalLoginRepositoryLazy;
    private readonly Lazy<IRoleRepository> _roleRepositoryLazy;
    private readonly Lazy<IClaimRepository> _claimRepositoryLazy;

    protected bool _disposed;


    public UnitOfWork()
    {
        _context = new EventsContext();

        _eventRepositoryLazy = new(() => new EventRepository(_context));
        _userRepositoryLazy = new(() => new UserRepository(_context));
        _externalLoginRepositoryLazy = new(() => new ExternalLoginRepository(_context));
        _roleRepositoryLazy = new(() => new RoleRepository(_context));
        _claimRepositoryLazy = new(() => new ClaimRepository(_context));
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


    public bool SaveChanges()
    {
        try
        {
            _context.SaveChanges(); 
            return true;
        }
        catch (DbUpdateException dbException)
        {
            return false;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException dbException)
        {
            return false;
        }
        catch (Exception exception)
        {
            return false;
        }
    }


    public void Dispose()
    {
        if (_disposed)
            return;

        _context.Dispose();
        
        _disposed = true;
    }
}
