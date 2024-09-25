using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace Events.DataBase.Repositories;


internal class RoleRepository : Repository<Role>, IRoleRepository
{
    internal RoleRepository(EventsContext context)
        : base(context)
    { }


    public IEnumerable<Role> GetUserRoles(int userId)
    {
        return Set.Where(r => r.Users.Any(u => u.Id == userId)).ToList();
    }

    public async IAsyncEnumerable<Role> GetUserRolesAsync(int userId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var role in Set.Where(r => r.Users.Any(u => u.Id == userId)).AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return role;
        }
    }
}