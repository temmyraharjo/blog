using System.Collections.Generic;
using Blog.Core.Core.Interface;
using Blog.Core.Models;

namespace Blog.Core.Core.Message
{
    public class PatchEntityRequest<TEntity> : IPatchEntityRequest
       where TEntity : Entity
    {
        public List<Microsoft.AspNetCore.JsonPatch.Operations.Operation> Operations { get; set; } = new List<Microsoft.AspNetCore.JsonPatch.Operations.Operation>();
        public long Id { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
