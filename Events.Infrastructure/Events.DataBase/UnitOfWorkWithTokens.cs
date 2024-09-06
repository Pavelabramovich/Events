using Events.DataBase.Repositories;
using Events.Domain;
using Events.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.DataBase;


public interface IUnitOfWorkWithTokens : IUnitOfWork
{
    IRefreshTokenRepository RefreshTokenRepository { get; }
}


public class UnitOfWorkWithTokens : UnitOfWork, IUnitOfWorkWithTokens
{
    private readonly Lazy<IRefreshTokenRepository> _refresTokenRepositoryLazy;


    public UnitOfWorkWithTokens()
        : base()
    {
        _refresTokenRepositoryLazy = new(() => new RefreshTokenRepository(_context));
    }

    public IRefreshTokenRepository RefreshTokenRepository
    {
        get => !_disposed
            ? _refresTokenRepositoryLazy.Value 
            : throw new ObjectDisposedException(nameof(UnitOfWork), "UnitOfWork is disposed.");
    }
}