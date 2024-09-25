using Events.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Events.Domain;
using Events.DataBase.Repositories;
using FluentAssertions.Execution;
using Events.Domain.Entities;
using Events.Domain.Enums;


namespace Events.Tests.RepositoriesTests;


public class Test_Repository : IDisposable
{
    private readonly SqliteConnection _fakeConnection;


    public Test_Repository()
    {
        _fakeConnection = new SqliteConnection("Filename=:memory:");
        _fakeConnection.Open();
    }

    public void Dispose()
    {
        _fakeConnection?.Dispose();
    }


    [Fact]
    public void Save_entities()
    {
        /// Arrange
        var dbContextOptions = new DbContextOptionsBuilder<EventsContext>()
            .UseSqlite(_fakeConnection)
            .Options;

        using var context = new EventsContext(dbContextOptions, hasData: false);

        context.Database.EnsureCreated();
        Repository<Event> repo = new EventRepository(context);


        /// Act
        Event[] events = 
        [
            new Event() { Id = 1, Name = "A", Category = Category.Meeting, Address = "AA", Description = "AAA", ImagePath = "AAAA", DateTime = DateTime.UtcNow },
            new Event() { Id = 2, Name = "B", Category = Category.Meeting, Address = "BB", Description = "BBB", ImagePath = "BBBB", DateTime = DateTime.UtcNow },
            new Event() { Id = 3, Name = "C", Category = Category.Meeting, Address = "CC", Description = "CCC", ImagePath = "CCCC", DateTime = DateTime.UtcNow },
        ];

        foreach (var @event in events)
        {
            repo.Add(@event);
        }

        context.SaveChanges();


        /// Assert
        var contextEvents = context.Events.ToArray();

        using (new AssertionScope())
        {
            foreach (var @event in events)
            {
                contextEvents.Should().ContainEquivalentOf(@event);
            }
        }
    }

    [Fact]
    public void Remove_entity()
    {
        /// Arrange
        var dbContextOptions = new DbContextOptionsBuilder<EventsContext>()
            .UseSqlite(_fakeConnection)
            .Options;

        using var context = new EventsContext(dbContextOptions, hasData: false);

        context.Database.EnsureCreated();
        Repository<Event> repo = new EventRepository(context);

        Event[] events =
        [
            new Event() { Id = 1, Name = "A", Category = Category.Meeting, Address = "AA", Description = "AAA", ImagePath = "AAAA" },
            new Event() { Id = 2, Name = "B", Category = Category.Meeting, Address = "BB", Description = "BBB", ImagePath = "BBBB" },
            new Event() { Id = 3, Name = "C", Category = Category.Meeting, Address = "CC", Description = "CCC", ImagePath = "CCCC" },
        ];

        foreach (var @event in events)
        {
            repo.Add(@event);
        }

        context.SaveChanges();


        /// Act
        repo.Remove(events[^1].Id);
        context.SaveChanges();


        /// Assert
        var contextEvents = context.Events.ToArray();

        using (new AssertionScope())
        {
            foreach (var @event in events[..^1])
            {
                contextEvents.Should().ContainEquivalentOf(@event);
            }

            contextEvents.Should().NotContainEquivalentOf(events[^1]);
        }
    }

    [Fact]
    public void Update_entity()
    {
        /// Arrange
        var dbContextOptions = new DbContextOptionsBuilder<EventsContext>()
            .UseSqlite(_fakeConnection)
            .Options;

        using var context = new EventsContext(dbContextOptions, hasData: false);

        context.Database.EnsureCreated();
        Repository<Event> repo = new EventRepository(context);

        var @event = new Event() { Id = 1, Name = "A", Category = Category.Meeting, Address = "AA", Description = "AAA", ImagePath = "AAAA" };

        repo.Add(@event);
        context.SaveChanges();


        /// Act
        var replacementEvent = new Event() { Id = @event.Id, Name = "B", Category = Category.Concert, Address = "BB", Description = "BBB", ImagePath = "BBBB" };
        repo.Update(replacementEvent);
        context.SaveChanges();


        /// Assert
        var targetEntity = context.Events.Single(e => e.Id == @event.Id);
        targetEntity.Should().BeEquivalentTo(replacementEvent);
    }

    [Fact]
    public void Get_entities()
    {
        /// Arrange
        var dbContextOptions = new DbContextOptionsBuilder<EventsContext>()
            .UseSqlite(_fakeConnection)
            .Options;

        using var context = new EventsContext(dbContextOptions, hasData: false);

        context.Database.EnsureCreated();
        Repository<Event> repo = new EventRepository(context);

        Event[] events =
        [
            new Event() { Id = 1, Name = "A", Category = Category.Meeting, Address = "AA", Description = "AAA", ImagePath = "AAAA" },
            new Event() { Id = 2, Name = "B", Category = Category.Meeting, Address = "BB", Description = "BBB", ImagePath = "BBBB" },
            new Event() { Id = 3, Name = "C", Category = Category.Meeting, Address = "CC", Description = "CCC", ImagePath = "CCCC" },
        ];

        foreach (var @event in events)
        {
            repo.Add(@event);
        }

        context.SaveChanges();


        /// Act
        var repoEvents = repo.GetAll();


        /// Assert
        using (new AssertionScope())
        {
            foreach (var @event in events)
            {
                repoEvents.Should().ContainEquivalentOf(@event);
            }
        }
    }
}
