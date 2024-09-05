using Events.Domain.Entities;


namespace Events.Domain.Repositories;


public interface IRepository<TEntity> where TEntity : IEntity
{
    List<TEntity> GetAll();
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    List<TEntity> PageAll(int skip, int take);
    Task<List<TEntity>> PageAllAsync(int skip, int take);
    Task<List<TEntity>> PageAllAsync(int skip, int take, CancellationToken cancellationToken);

    TEntity? FindById(object id);
    Task<TEntity?> FindByIdAsync(object id);
    Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
