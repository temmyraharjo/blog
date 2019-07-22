using System.Collections.Generic;
using MediatR;

namespace Blog.Core.Core.Interface
{
    public interface IPatchEntityRequest : IRequest<bool>
    {
        List<Microsoft.AspNetCore.JsonPatch.Operations.Operation> Operations { get; set; }

        long Id { get; set; }

        byte[] RowVersion { get; set; }
    }
}
