using LearningCqrs.Contracts;
using MediatR;

namespace LearningCqrs.Core;

public static class EntityExtensions
{
    public static async Task<IEntity> SetAuditProperty(this IEntity entity, IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken, bool isCreate = true)
    {
        var currentUser = await httpContextAccessor.GetCurrentUser(mediator, cancellationToken);

        await entity.SetAuditProperty(currentUser, isCreate);

        return entity;
    }

    public static async Task<IEntity[]> SetAuditProperty(this IEntity[] entities,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken, IUser? user = null, bool isCreate = true)
    {
        var currentUser = user ?? await httpContextAccessor.GetCurrentUser(mediator, cancellationToken);
        foreach (var entity in entities) await entity.SetAuditProperty(currentUser, isCreate);

        return entities;
    }

    private static Task SetAuditProperty(this IEntity entity, IUser? user = null, bool isCreate = true)
    {
        if (entity is not IAuditEntity auditEntity) return Task.CompletedTask;

        if (isCreate)
        {
            auditEntity.CreatedOn = DateTime.UtcNow;
            auditEntity.CreatedById = user?.Id;
        }

        auditEntity.ModifiedOn = DateTime.UtcNow;
        auditEntity.ModifiedById = user?.Id;

        return Task.CompletedTask;
    }
}