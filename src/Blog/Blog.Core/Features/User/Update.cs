using AutoMapper;
using Blog.Core.Core.Message;
using Blog.Core.Core.Operation;
using Blog.Core.Data;
using FluentValidation;

namespace Blog.Core.Features.User
{
    public class Update
    {
        public class Command : PatchEntityRequest<Models.User>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Repassword { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(e => e.Password).Equal(e => e.Repassword);
            }
        }

        public class Handler : PatchEntityHandlerBase<Models.User, Command>
        {
            public Handler(BlogContext context, IMapper mapper) : 
                base(context, mapper)
            {
            }
        }
    }
}
