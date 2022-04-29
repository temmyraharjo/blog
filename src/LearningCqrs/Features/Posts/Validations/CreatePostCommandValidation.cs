using FluentValidation;
using LearningCqrs.Contracts;

namespace LearningCqrs.Features.Posts.Validations;

public class CreatePostCommandValidator : AbstractValidator<Create.CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        When(x => x.Status == Status.Published && !x.PublishedAt.HasValue,
            () => { RuleFor(x => x.PublishedAt).NotNull(); });
    }
}