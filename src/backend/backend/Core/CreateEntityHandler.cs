using AutoMapper;
using backend.Data;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace backend.Core
{
    public class CreateEntityHandler<TEntity, TRequest> : IRequestHandler<TRequest, Guid>
        where TEntity : Entity, new()
        where TRequest : IRequest<Guid>
    {
        public BlogContext Context { get; }
        public IMapper Mapper { get; }

        public CreateEntityHandler(BlogContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual async Task<Guid> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var record = Mapper.Map<TRequest, TEntity>(request);
            var entry = await Context.Set<TEntity>().AddAsync(record, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return entry.Entity.Id;
        }
    }
}
