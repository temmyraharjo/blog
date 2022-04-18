using LearningCqrs.Contracts;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Core;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IEntity
{
    public BlogContext Context { get; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(BlogContext context, IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        Context = context;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
        _dbSet = Context.Set<TEntity>();
    }

    private async Task<User?> GetCurrentUser(CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (string.IsNullOrEmpty(currentUser)) return null;
        
        return await _mediator.Send(new GetByUsername.GetByUsernameQuery(currentUser), cancellationToken);
    }
    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await SetAuditProperties(entity, cancellationToken);
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    private async Task SetAuditProperties(TEntity entity, CancellationToken cancellationToken)
    {
        if (entity is IAuditEntity auditEntity)
        {
            var currentUser = await GetCurrentUser(cancellationToken);
            auditEntity.CreatedOn = DateTime.UtcNow;
            auditEntity.CreatedById = currentUser?.Id;
            auditEntity.ModifiedOn = DateTime.UtcNow;
            auditEntity.ModifiedById = currentUser?.Id;
        }
    }

    public void Update(TEntity entity)
    {
        SetAuditProperties(entity, CancellationToken.None).GetAwaiter().GetResult();
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