using FluentValidation;

namespace LearningCqrs.Features.Users.Validations;

public class AuthorizeCommandValidator : AbstractValidator<Authorize.AuthorizeCommand>
{
    public AuthorizeCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}