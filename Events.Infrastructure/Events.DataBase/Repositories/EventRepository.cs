﻿using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace Events.DataBase.Repositories;


internal class EventRepository : Repository<Event>, IEventRepository 
{
    public EventRepository(EventsContext context)
        : base(context)
    { }


    public IEnumerable<Event> GetAllWithParticipations()
    {
        return Set.AsNoTracking().Include(e => e.Participants).ThenInclude(p => p.User).ToArray();
    }

    public async IAsyncEnumerable<Event> GetAllWithParticipationsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var events = Set
            .AsNoTracking()
            .Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken);

        await foreach (var @event in events)
        {
            yield return @event;
        }
    }


    public IEnumerable<Participation> GetEventParticipations(int eventId)
    {
        return Set.AsNoTracking().Where(e => e.Id == eventId).Include(e => e.Participants).ThenInclude(p => p.User).Single().Participants.ToArray();
    }

    public async IAsyncEnumerable<Participation> GetEventParticipationsAsync(int eventId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var participants = Set
            .AsNoTracking()
            .Where(e => e.Id == eventId)
            .Include(e => e.Participants)
                .ThenInclude(p => p.User)
            .SelectMany(e => e.Participants)
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken);

        await foreach (var participation in participants)
        { 
            yield return participation;
        }
    }


    public void AddParticipant(int eventId, int userId)
    {
        var @event = Set.FirstOrDefault(e => e.Id == eventId)!;

        @event.Participants.Add(new Participation() { UserId = userId, RegistrationTime = DateTime.UtcNow });
    }

    public async Task AddParticipantAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        var @event = await Set.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        @event!.Participants.Add(new Participation { UserId = userId, RegistrationTime = DateTime.UtcNow });
    }


    public void RemoveParticipant(int eventId, int userId)
    {
        var @event = Set.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId)!;

        var participation = @event.Participants.FirstOrDefault(p => p.UserId == userId)!;

        @event.Participants.Remove(participation);
    }

    public async Task RemoveParticipantAsync(int eventId, int userId, CancellationToken cancellationToken = default)
    {
        var @event = await Set.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        var participation = @event!.Participants.FirstOrDefault(p => p.UserId == userId)!;

        @event.Participants.Remove(participation);
    }

    public int ParticipantsCount(int eventId)
    {
        var @event = Set.Include(e => e.Participants).FirstOrDefault(e => e.Id == eventId)!;

        return @event.Participants.Count; 
    }

    public async Task<int> ParticipantsCountAsync(int eventId, CancellationToken cancellationToken = default)
    {
        var @event = await Set.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        return @event!.Participants.Count;
    }


    public Event? FindByName(string name)
    {
        return Set.FirstOrDefault(e => e.Name == name);
    }

    public Task<Event?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Set.FirstOrDefaultAsync(e => e.Name == name, cancellationToken);
    }
}
