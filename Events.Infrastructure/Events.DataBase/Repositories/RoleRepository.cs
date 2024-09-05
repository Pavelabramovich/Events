using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Events.DataBase.Repositories;


internal class RoleRepository : Repository<Role>, IRoleRepository
{
    internal RoleRepository(EventsContext context)
        : base(context)
    { }

    public Role? FindByName(string name)
    {
        return Set.FirstOrDefault(r => r.Name == name);
    }

    public Task<Role?> FindByNameAsync(string name)
    {
        return Set.FirstOrDefaultAsync(x => x.Name == name);
    }

    public Task<Role?> FindByNameAsync(string roleName, CancellationToken cancellationToken)
    {
        return Set.FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);
    }
}