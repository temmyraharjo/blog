using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace LearningCqrs.Core.Handler;

public record UpdateDocument<TInput>(Guid Id, JsonPatchDocument<TInput> JsonPatchDocument) 
    : IRequest<Unit> where TInput : class, IRequest<Unit>;