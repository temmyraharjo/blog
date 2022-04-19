using LearningCqrs.Contracts;
using LearningCqrs.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Core;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    public BlogContext Context { get; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(BlogContext context, IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        Context = context;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
        _dbSet = Context.Set<TEntity>();
    }
    
    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await entity.SetAuditProperty(_httpContextAccessor, _mediator, cancellationToken);
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        entity.SetAuditProperty(_httpContextAccessor, _mediator, CancellationToken.None, false).GetAwaiter().GetResult();
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }
}