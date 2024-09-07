using Events.Domain;
using Events.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace Events.DataBase.Repositories;


internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly EventsContext _context;


    internal Repository(EventsContext context)
    {
        _context = context;
    }

    protected DbSet<TEntity> Set => _context.Set<TEntity>();


    public IEnumerable<TEntity> GetAll()
    {
        return Set.AsNoTracking().ToArray();
    }

    public async IAsyncEnumerable<TEntity> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var entity in Set.AsNoTracking().AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return entity;
        }
    }


    public IEnumerable<TEntity> PageAll(int skip, int take)
    {
        return Set.AsNoTracking().Skip(skip).Take(take).ToArray();
    }

    public async IAsyncEnumerable<TEntity> PageAllAsync(int skip, int take, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var entity in Set.AsNoTracking().Skip(skip).Take(take).AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return entity;
        }
    }


    public TEntity? FindById(object id)
    {
        return Set.Find(id);
    }

    public Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return Set.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }


    public void Add(TEntity entity)
    {
        Set.Add(entity);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException
            .ThrowIfNull(entity, nameof(entity));

        var existedEntity = Set.FirstOrDefault(e => e.Id == entity.Id)
            ?? throw new InvalidOperationException("Entity not found.");

        _context.Entry(existedEntity).CurrentValues.SetValues(entity);
    }

    public void Remove(object id)
    {
        var entity = Set.Find(id);

        if (entity is not null)
            Set.Remove(entity);
    }

    public int Count()
    {
        return Set.Count();
    }

    public Task<int> CountAsync()
    {
        return Set.CountAsync();
    }
}
