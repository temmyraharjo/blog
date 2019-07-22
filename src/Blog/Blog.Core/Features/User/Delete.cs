using AutoMapper;
using Blog.Core.Core.Message;
using Blog.Core.Core.Operation;
using Blog.Core.Data;

namespace Blog.Core.Features.User
{
    public class Delete
    {
        public class Command : DeleteEntityRequest<Models.User>
        {
        }

        public class Handler : DeleteEntityHandlerBase<Models.User, Command>
        {
            public Handler(BlogContext context, IMapper mapper) : 
                base(context, mapper)
            {
            }
        }
    }
}
