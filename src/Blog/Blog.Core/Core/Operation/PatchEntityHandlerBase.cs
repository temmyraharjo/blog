using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Core.Interface;
using Blog.Core.Data;
using Blog.Core.Models;

namespace Blog.Core.Core.Operation
{
    public abstract class PatchEntityHandlerBase<TEntity, TRequest> : 
        BaseEntityHandler<TEntity, TRequest, bool>
       where TEntity : Entity, new()
       where TRequest : IPatchEntityRequest
    {
        public PatchEntityHandlerBase(BlogContext context, IMapper mapper) : 
            base(context, mapper)
        {
        }

        public override async Task<bool> Handling(TRequest request,
            CancellationToken cancellationToken)
        {
            var patch = Mapper.Map<TRequest, TEntity>(request);
            var patchEntry = Context.Attach(patch);
            var propertyNames = request.Operations.Select(op => op.GetPropertyName());
            foreach (var propertyName in propertyNames)
                patchEntry.Property(propertyName).IsModified = true;

            var affected = await Context.SaveChangesAsync(cancellationToken);
            return affected > 0;
        }
    }
}
