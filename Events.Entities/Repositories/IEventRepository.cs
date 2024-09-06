using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IEventRepository : IRepository<Event>
{
    IEnumerable<Event> GetAllWithParticipations();
    IAsyncEnumerable<Event> GetAllWithParticipationsAsync(CancellationToken cancellationToken = default);

    IEnumerable<Participation> GetEventParticipants(int eventId);
    IAsyncEnumerable<Participation> GetEventParticipationsAsync(int eventId, CancellationToken cancellationToken = default);

    void AddParticipant(int eventId, int userId);
    Task AddParticipantAsync(int eventId, int userId, CancellationToken cancellationToken = default);

    void RemoveParticipant(int eventId, int userId);
    Task RemoveParticipantAsync(int eventId, int userId, CancellationToken cancellationToken = default);

    Event? FindByName(string name);
    Task<Event?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
