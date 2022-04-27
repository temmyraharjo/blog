using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace LearningCqrs.Core.Handler;

public abstract class UpdateDocumentHandler<TInput, TEntity> : IRequestHandler<UpdateDocument<TInput, TEntity>, TEntity>
    where TInput : class, IRequest<TEntity>
    where TEntity : class, IEntity
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;
    protected readonly IEnumerable<IValidator<TInput>> Validators;

    public UpdateDocumentHandler(IRepository<TEntity> repository, IMapper mapper, IEnumerable<IValidator<TInput>> validators)
    {
        Repository = repository;
        Mapper = mapper;
        Validators = validators;
    }

    public virtual async Task<TEntity> Handle(UpdateDocument<TInput, TEntity> request,
        CancellationToken cancellationToken)
    {
        var entity = await Repository.Context.Set<TEntity>()
            .SingleAsync(x => x.Id == request.Id , 
                cancellationToken: cancellationToken);
        var valid = await new RowVersionValidator<TEntity>(request.RowVersion).ValidateAsync(entity, cancellationToken);
        if (!valid.IsValid)
        {
            var error = string.Join("\r\n", valid.Errors);
            throw new AggregateException("Model Error: " + error);
        }
        
        return await Handling(entity, request, cancellationToken);
    }

    public virtual async Task<TEntity> Handling(TEntity entity, UpdateDocument<TInput, TEntity> request,
        CancellationToken cancellationToken)
    {
        var initialDto = Mapper.Map<TInput>(entity);
        request.JsonPatchDocument.ApplyTo(initialDto);
        Validate(initialDto);
        Mapper.Map(initialDto, entity);

        Repository.Update(entity);
        await Repository.SaveChangesAsync(cancellationToken);

        return entity;
    }

    private void Validate(TInput request)
    {
        var validationFailures = Validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null).ToList();

        if (validationFailures.Any()) throw new ApiValidationException(validationFailures.ToArray());
    }
}