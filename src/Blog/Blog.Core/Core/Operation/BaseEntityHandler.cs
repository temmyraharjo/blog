using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Data;
using Blog.Core.Models;
using MediatR;

namespace Blog.Core.Core.Operation
{
    public abstract class BaseEntityHandler<TEntity, TRequest, TReturn> : IRequestHandler<TRequest, TReturn>
        where TEntity : Entity, new()
        where TRequest : IRequest<TReturn>
    {
        public BlogContext Context { get; }
        public IMapper Mapper { get; }

        protected BaseEntityHandler(BlogContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<TReturn> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Context.BeginTransactionAsync(cancellationToken);
                var result = await Handling(request, cancellationToken);
                Context.CommitTransaction();

                return result;
            }
            catch
            {
                Context.RollBackTransaction();
                throw;
            }
        }

        public abstract Task<TReturn> Handling(TRequest request, CancellationToken cancellationToken);
    }
}
