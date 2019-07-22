using System.Collections.Generic;

namespace Blog.Core.GraphQl.Models
{
    public interface IDatabaseMetadata
    {
        void ReloadMetadata();
        IEnumerable<TableMetadata> GetTableMetadatas();
    }
}
