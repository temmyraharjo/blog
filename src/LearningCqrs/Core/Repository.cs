using System.Reflection;
using AutoMapper.Internal;
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
        await SetEntityOrCollectionEntityAuditProperty(entity, _httpContextAccessor, _mediator, cancellationToken);
        await entity.SetAuditProperty(_httpContextAccessor, _mediator, cancellationToken);
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    private async Task SetEntityOrCollectionEntityAuditProperty(TEntity entity,
        IHttpContextAccessor httpContextAccessor, IMediator mediator, CancellationToken cancellationToken,
        bool isCreate = true)
    {
        const BindingFlags bindingFlags = BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;
        var validProperties = entity.GetType()
            .GetProperties(bindingFlags)
            .Where(p => p.CanRead && p.CanWrite)
            .Where(p => p.PropertyType.IsAssignableFrom(typeof(IAuditEntity)) ||
                        p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
            .ToArray();

        foreach (var property in validProperties)
        {
            var value = property.GetMemberValue(entity);
            if (value == null) continue;

            await SetEntity(value, cancellationToken, isCreate);
            await SetEntityCollection(value, cancellationToken, isCreate);
        }
    }

    private async Task SetEntityCollection(object value,
        CancellationToken cancellationToken, bool isCreate = true)
    {
        var entities = value as ICollection<IEntity>;
        if (entities == null) return;

        foreach (var entity in entities)
            await entity.SetAuditProperty(_httpContextAccessor, _mediator, cancellationToken, isCreate);
    }

    private async Task SetEntity(object value,
        CancellationToken cancellationToken, bool isCreate = true)
    {
        var entity = value as IEntity;
        if (entity == null) return;

        await entity.SetAuditProperty(_httpContextAccessor, _mediator, cancellationToken, isCreate);
    }

    public void Update(TEntity entity)
    {
        SetEntityOrCollectionEntityAuditProperty(entity, _httpContextAccessor, _mediator, CancellationToken.None, false)
            .GetAwaiter().GetResult();
        entity.SetAuditProperty(_httpContextAccessor, _mediator, CancellationToken.None, false).GetAwaiter()
            .GetResult();
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