using FluentValidation;

namespace LearningCqrs.Features.Categories.Validations;

public class UpdateCategoryCommandValidator : AbstractValidator<Update.UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}