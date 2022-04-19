using FluentValidation;
using LearningCqrs.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Features.Categories.Validations;

public class CreateCategoryCommandValidator : AbstractValidator<Create.CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(BlogContext context)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x=>x.Name).CustomAsync(async (name, validationContext, cancellationToken) =>
        {
            var exists = await context.Categories.Where(x => x.Name == name)
                .AnyAsync(cancellationToken);
            if (!exists) return;

            validationContext.AddFailure($"Category '{name}' is already exist.");
        });
    }
}