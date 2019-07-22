using Blog.Core.Core.Interface;

namespace Blog.Core.Core.Message
{
    public class DeleteEntityRequest<TEntity> : IDeleteEntityRequest<TEntity>
    {
        public long Id { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
