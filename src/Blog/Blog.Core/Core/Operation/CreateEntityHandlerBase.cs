using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Data;
using Blog.Core.Models;
using MediatR;

namespace Blog.Core.Core.Operation
{
    public abstract class CreateEntityHandlerBase<TEntity, TRequest> : 
        BaseEntityHandler<TEntity, TRequest, long>
        where TEntity : Entity, new()
        where TRequest : IRequest<long>
    {
        protected CreateEntityHandlerBase(BlogContext context, IMapper mapper) : 
            base(context, mapper)
        {
        }

        public override async Task<long> Handling(TRequest request,
            CancellationToken cancellationToken)
        {
            var record = Mapper.Map<TRequest, TEntity>(request);
            var result = await Context.Set<TEntity>().AddAsync(record, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return result.Entity.Id;
        }
    }
}
