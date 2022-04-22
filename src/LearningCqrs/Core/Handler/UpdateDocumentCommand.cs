using LearningCqrs.Contracts;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace LearningCqrs.Core.Handler;

public record UpdateDocumentCommand<TInput, TEntity>(JsonPatchDocument<TInput> Patches, 
    string? Version = null): IRequest<TEntity> 
    where TInput : class, IRequest<TEntity>
    where TEntity: IEntity;