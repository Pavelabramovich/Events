using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Events.Entities;


public interface IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
{
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    IEnumerable<TEntity> PageAll(int skip, int take);
    Task<IEnumerable<TEntity>> PageAllAsync(int skip, int take);
    Task<IEnumerable<TEntity>> PageAllAsync(int skip, int take, CancellationToken cancellationToken);

    TEntity FindById(TKey id);
    Task<TEntity> FindByIdAsync(TKey id);
    Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
