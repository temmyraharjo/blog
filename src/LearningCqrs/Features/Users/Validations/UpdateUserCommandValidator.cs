using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Data;

namespace LearningCqrs.Features.Users.Validations;

public class UpdateUserCommandValidator : AbstractValidator<Update.UpdateUserCommand>
{
    private readonly IRepository<User> _repository;

    public UpdateUserCommandValidator(IRepository<User> repository)
    {
        When(x => x.Password != null, () =>
        {
            RuleFor(x => x.Password).NotEmpty();
        });
    }
}