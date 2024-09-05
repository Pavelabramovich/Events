using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.DataBase.Repositories;


internal class UserRepository : Repository<User>, IUserRepository
{
    internal UserRepository(EventsContext context)
        : base(context)
    { }


    public User? FindByName(string name)
    {
        return Set.FirstOrDefault(u => u.Name == name);
    }

    public Task<User?> FindByNameAsync(string name)
    {
        return Set.FirstOrDefaultAsync(u => u.Name == name);
    }

    public Task<User?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return Set.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
    }


    public User? FindByLogin(string login)
    {
        return Set.FirstOrDefault(u => u.Login == login);
    }

    public Task<User?> FindByLoginAsync(string login)
    {
        return Set.FirstOrDefaultAsync(u => u.Login == login);
    }

    public Task<User?> FindByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return Set.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }
}