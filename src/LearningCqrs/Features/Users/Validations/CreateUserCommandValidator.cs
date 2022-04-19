using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Features.Users.Validations;

public class CreateUserCommandValidator : AbstractValidator<Create.CreateUserCommand>
{
    public CreateUserCommandValidator(IRepository<User> repository)
    {
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Password).NotNull();
        RuleFor(x => x.Username).CustomAsync(async (username, validationContext, cancellationToken) =>
        {
            var exists = await repository.Context.Users.Where(x => x.Username == username)
                .AnyAsync(cancellationToken);

            if (!exists) return;

            validationContext.AddFailure($"Username '{username}' is already exist.");
        });
    }
}