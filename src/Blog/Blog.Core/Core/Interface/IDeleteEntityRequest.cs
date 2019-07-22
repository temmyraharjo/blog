using System;
using MediatR;

namespace Blog.Core.Core.Interface
{
    public interface IDeleteEntityRequest<TEntity> : IRequest<bool>
    {
        long Id { get; set; }
        byte[] RowVersion { get; set; }
    }
}
