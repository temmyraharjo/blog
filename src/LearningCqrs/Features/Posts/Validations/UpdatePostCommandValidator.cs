using FluentValidation;
using LearningCqrs.Contracts;

namespace LearningCqrs.Features.Posts.Validations;

public class UpdatePostCommandValidator : AbstractValidator<Update.UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        When(x => x.Status == Status.Published && !x.PublishedAt.HasValue,
            () => { RuleFor(x => x.PublishedAt).NotNull(); });
    }
}