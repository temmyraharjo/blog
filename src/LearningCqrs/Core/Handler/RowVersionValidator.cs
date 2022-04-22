using FluentValidation;
using LearningCqrs.Contracts;

namespace LearningCqrs.Core.Handler;

public class RowVersionValidator<TEntity> : AbstractValidator<TEntity>
    where TEntity : IEntity
{
    public RowVersionValidator(string? rowVersion)
    {
        RuleFor(x => x).Custom((entity, validationContext) =>
        {
            var valid = entity.RowVersion == null || Convert.ToBase64String(entity.RowVersion) == rowVersion;
            if (valid) return;
            
            validationContext.AddFailure($"Update {typeof(TEntity).Name} with id '{entity.Id}' is failed. Please retry again later.");
        });
    }
}