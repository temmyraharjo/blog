using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Data;
using Blog.Core.Models;
using MediatR;

namespace Blog.Core.Core.Operation
{
    public abstract class DeleteEntityHandlerBase<TEntity, TRequest> :
        BaseEntityHandler<TEntity, TRequest, bool>
        where TEntity : Entity, new()
        where TRequest : IRequest<bool>
    {
        protected DeleteEntityHandlerBase(BlogContext context, IMapper mapper) : 
            base(context, mapper)
        {
        }

        public override async Task<bool> Handling(TRequest request, 
            CancellationToken cancellationToken)
        {
            var record = Mapper.Map<TRequest, TEntity>(request);
            Context.Remove(record);

            var rowAffected = await Context.SaveChangesAsync(cancellationToken);
            return await Task.Run(() => rowAffected > 0, cancellationToken);
        }
    }
}
