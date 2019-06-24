using backend.Data;
using FluentValidation;
using MediatR;
using System;
using backend.Features.User;
using System.Threading;
using System.Threading.Tasks;
using backend.Core;
using AutoMapper;

namespace backend.Features
{
    public class Create
    {
        public class Command : IRequest<Guid>
        {
            public Guid Id { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(BlogContext context)
            {
                RuleFor(m => m.UserName).NotNull();
                RuleFor(m => m.FirstName).NotNull();
                RuleFor(m => m.LastName).NotNull();
                RuleFor(m => m).UserNameUnique(context);
            }
        }

        public class Handler : CreateEntityHandler<Command, Data.User>
        {
            public Handler(BlogContext context, IMapper mapper) : 
                base(context, mapper)
            {

            }
        }
    }
}
