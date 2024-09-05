using Events.Domain.Entities;
using Events.Domain.Repositories;


namespace Events.DataBase.Repositories;


internal class EventRepository : Repository<Event>, IEventRepository 
{
    public EventRepository(EventsContext context)
        : base(context)
    { }
}
