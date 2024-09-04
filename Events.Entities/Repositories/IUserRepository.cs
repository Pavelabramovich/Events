using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public interface IUserRepository : IRepository<User>
{
    User FindByUserName(string username);
    Task<User> FindByUserNameAsync(string username);
    Task<User> FindByUserNameAsync(string username, CancellationToken cancellationToke);

    User FindByEmail(string email);
    Task<User> FindByEmailAsync(string email);
    Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken);
}