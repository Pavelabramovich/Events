using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IRoleRepository : IRepository<Role>
{
    Role? FindByName(string name);
    Task<Role?> FindByNameAsync(string name);
    Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken);
}
