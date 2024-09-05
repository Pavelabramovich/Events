using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public interface IUserRepository : IRepository<User, int>
{
    User FindByName(string name);
    Task<User> FindByNameAsync(string name);
    Task<User> FindByNameAsync(string name, CancellationToken cancellationToke);

    User FindByEmail(string email);
    Task<User> FindByEmailAsync(string email);
    Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken);
}