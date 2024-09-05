using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IUserRepository : IRepository<User>
{
    User? FindByName(string name);
    Task<User?> FindByNameAsync(string name);
    Task<User?> FindByNameAsync(string name, CancellationToken cancellationToke);

    User? FindByLogin(string login);
    Task<User?> FindByLoginAsync(string login);
    Task<User?> FindByLoginAsync(string login, CancellationToken cancellationToken);

    /// some methods for events and participations...
}
