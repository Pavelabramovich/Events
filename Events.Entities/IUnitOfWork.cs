using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IExternalLoginRepository ExternalLoginRepository { get; }
    IRoleRepository RoleRepository { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
