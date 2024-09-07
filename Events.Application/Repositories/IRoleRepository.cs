using Events.Domain;


namespace Events.Application.Repositories;


public interface IRoleRepository : IRepository<Role>
{
    IEnumerable<Role> GetUserRoles(int userId);
    IAsyncEnumerable<Role> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
}
