using Events.DataBase.Repositories;
using Events.Domain.Repositories;
using Events.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Events.DataBase;


public class UnitOfWork : IUnitOfWork
{
    private readonly EventsContext _context;

    private IEventRepository? _eventRepository;
    private IUserRepository? _userRepository;
    private IExternalLoginRepository? _externalLoginRepository;
    private IRoleRepository? _roleRepository;
    


    public UnitOfWork(Action<DbContextOptionsBuilder>? configureAction = null)
    {
        if (configureAction is null)
        {
            _context = new EventsContext();
        }
        else
        {
            var builder = new DbContextOptionsBuilder<EventsContext>();
            configureAction(builder);
            DbContextOptions<EventsContext> options = builder.Options;

            _context = new EventsContext(options);
        }

        _eventRepository = null;
        _userRepository = null;
        _externalLoginRepository = null;
        _roleRepository = null;
    }


    public IEventRepository EventRepository
    {
        get => _eventRepository ??= new EventRepository(_context);
    }

    public IUserRepository UserRepository
    {
        get => _userRepository ??= new UserRepository(_context);
    }

    public IExternalLoginRepository ExternalLoginRepository
    {
        get  => _externalLoginRepository ??= new ExternalLoginRepository(_context); 
    }

    public IRoleRepository RoleRepository
    {
        get => _roleRepository ??= new RoleRepository(_context); 
    }


    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }


    public void Dispose()
    {
        _eventRepository = null;
        _userRepository = null;
        _externalLoginRepository = null;
        _roleRepository = null;
        
        _context.Dispose();
    }
}
