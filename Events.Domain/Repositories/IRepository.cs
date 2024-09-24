using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IRepository<TEntity> where TEntity : IEntity
{
    IEnumerable<TEntity> GetAll();
    IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default);

    IEnumerable<TEntity> PageAll(int skip, int take);
    IAsyncEnumerable<TEntity> PageAllAsync(int skip, int take, CancellationToken cancellationToken = default);

    TEntity? FindById(object id);
    Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken = default);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(object id);

    int Count();
    Task<int> CountAsync();
}
