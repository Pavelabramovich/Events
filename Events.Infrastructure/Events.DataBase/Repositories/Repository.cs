using Events.Domain.Entities;
using Events.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.DataBase.Repositories;


internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly EventsContext _context;

    internal Repository(EventsContext context)
    {
        _context = context;
    }

    protected DbSet<TEntity> Set => _context.Set<TEntity>();


    public List<TEntity> GetAll()
    {
        return Set.ToList();
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return Set.ToListAsync();
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Set.ToListAsync(cancellationToken);
    }


    public List<TEntity> PageAll(int skip, int take)
    {
        return Set.Skip(skip).Take(take).ToList();
    }

    public Task<List<TEntity>> PageAllAsync(int skip, int take)
    {
        return Set.Skip(skip).Take(take).ToListAsync();
    }

    public Task<List<TEntity>> PageAllAsync(int skip, int take, CancellationToken cancellationToken)
    {
        return Set.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }


    public TEntity? FindById(object id)
    {
        return Set.Find(id);
    }

    public Task<TEntity?> FindByIdAsync(object id)
    {
        return Set.FirstOrDefaultAsync(e => e.Id == id);    
    }

    public Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken)
    {
        return Set.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }


    public void Add(TEntity entity)
    {
        Set.Add(entity);
    }

    public void Update(TEntity entity)
    {
        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            Set.Attach(entity);
            entry = _context.Entry(entity);
        }

        entry.State = EntityState.Modified;
    }

    public void Remove(TEntity entity)
    {
        Set.Remove(entity);
    }
}
