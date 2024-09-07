using Events.Domain;


namespace Events.Application.Repositories;


public interface IUserRepository : IRepository<User>
{
    User? FindByName(string name);
    Task<User?> FindByNameAsync(string name, CancellationToken cancellationToke = default);

    User? FindByLogin(string login);
    Task<User?> FindByLoginAsync(string login, CancellationToken cancellationToken = default);

    IEnumerable<User> GetAllWithParticipations();
    IAsyncEnumerable<User> GetAllWithParticipationsAsync(CancellationToken cancellationToken = default);

    bool Authenticate(string login, string hashedPassword);
    Task<bool> AuthenticateAsync(string login, string hashedPassword, CancellationToken cancellationToken = default);

    IEnumerable<Participation> GetUserEvents(int userId);
    IAsyncEnumerable<Participation> GetUserEventsAsync(int userId, CancellationToken cancellationToken = default);
}
