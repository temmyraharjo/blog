using System.Collections.Generic;

namespace Blog.Core.GraphQl.Models
{

    public class TableMetadata
    {
        public string TableName { get; set; }
        public string AssemblyFullName { get; set; }
        public IEnumerable<ColumnMetadata> Columns { get; set; }
    }
}
