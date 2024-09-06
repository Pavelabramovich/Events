using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;


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

    public Task<User?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Set.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
    }


    public User? FindByLogin(string login)
    {
        return Set.FirstOrDefault(u => u.Login == login);
    }

    public Task<User?> FindByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return Set.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }


    public IEnumerable<User> GetAllWithParticipations()
    {
        return Set.Include(e => e.Events).ToArray();
    }

    public IAsyncEnumerable<User> GetAllWithParticipationsAsync(CancellationToken cancellationToken = default)
    {
        return Set.Include(e => e.Events).AsAsyncEnumerable();
    }


    public bool Authenticate(string login, string hashedPassword)
    {
        return Set.Any(u => u.Login == login && u.HashedPassword == hashedPassword);
    }

    public Task<bool> AuthenticateAsync(string login, string hashedPassword, CancellationToken cancellationToken = default)
    {
        return Set.AnyAsync(u => u.Login == login && u.HashedPassword == hashedPassword, cancellationToken);
    }

    public IEnumerable<Participation> GetUserEvents(int userId)
    {
        return Set.AsNoTracking().Where(u => u.Id == userId).Include(e => e.Participants).ThenInclude(p => p.Event).Single().Participants.ToArray();
    }

    public async IAsyncEnumerable<Participation> GetUserEventsAsync(int userId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var participants = Set
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Include(e => e.Participants)
                .ThenInclude(p => p.Event)
            .SelectMany(u => u.Participants)
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken);

        await foreach (var participation in participants)
        {
            yield return participation;
        }
    }
}