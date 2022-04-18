using LearningCqrs.Data;

namespace LearningCqrs.Contracts;

public interface IGenericRepository<TEntity>
    where TEntity : IEntity
{
    BlogContext Context { get; }
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}