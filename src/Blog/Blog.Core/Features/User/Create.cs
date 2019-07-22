using System;
using AutoMapper;
using Blog.Core.Core.Operation;
using Blog.Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Features.User
{
    public class Create
    {
        public class Command : IRequest<long>
        {
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string Repassword { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(BlogContext dbContext)
            {
                RuleFor(e => e.FirstName).NotNull().MaximumLength(50);
                RuleFor(e => e.LastName).NotNull().MaximumLength(50);
                RuleFor(e => e.Password).NotNull();
                RuleFor(e => e.Repassword).NotNull();
                RuleFor(e => e.Password).Equal(e => e.Repassword);

                RuleFor(e => e.UserName).MustAsync(async (userName, cancellation) =>
                {
                    var isExists = await dbContext.Users.
                        AnyAsync(e => e.UserName == userName, cancellation);
                    return !isExists;
                }).OverridePropertyName("UsernameNotUnique")
                .WithMessage(e => $"Username {e.UserName} already exists!");
            }
        }

        public class Handler : CreateEntityHandlerBase<Models.User, Command>
        {
            public Handler(BlogContext context, IMapper mapper) :
                base(context, mapper)
            {
            }
        }
    }
}
