
namespace Events.Entities;


public interface IRoleRepository : IRepository<Role, string>
{
    Role FindByName(string name);
    Task<Role> FindByNameAsync(string name);
    Task<Role> FindByNameAsync(string name, CancellationToken cancellationToken);
}
