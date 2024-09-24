using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IRoleRepository : IRepository<Role>
{
    IEnumerable<Role> GetUserRoles(int userId);
    IAsyncEnumerable<Role> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
}
