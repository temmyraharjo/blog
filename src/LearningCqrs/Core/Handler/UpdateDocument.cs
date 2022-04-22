using LearningCqrs.Contracts;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace LearningCqrs.Core.Handler;

public record UpdateDocument<TInput, TEntity>(Guid Id, JsonPatchDocument<TInput> JsonPatchDocument,
        string? RowVersion = null) : IRequest<TEntity>
    where TInput : class, IRequest<TEntity>
    where TEntity : IEntity;