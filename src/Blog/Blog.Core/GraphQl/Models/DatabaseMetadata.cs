using System.Collections.Generic;
using Blog.Core.Data;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.GraphQl.Models
{
    public sealed class DatabaseMetadata : IDatabaseMetadata
    {
        private readonly BlogContext _dbContext;
        private readonly ITableNameLookup _tableNameLookup;
        private IEnumerable<TableMetadata> _tables;

        public DatabaseMetadata(BlogContext dbContext, ITableNameLookup tableNameLookup)
        {
            _dbContext = dbContext;
            _tableNameLookup = tableNameLookup;
            if (_tables == null)
                ReloadMetadata();
        }

        public IEnumerable<TableMetadata> GetTableMetadatas()
        {
            if (_tables == null)
                return new List<TableMetadata>();
            return _tables;
        }

        public void ReloadMetadata()
        {
            _tables = FetchTableMetaData();
        }

        private IReadOnlyList<TableMetadata> FetchTableMetaData()
        {
            var metaTables = new List<TableMetadata>();
            foreach (var entityType in _dbContext.Model.GetEntityTypes())
            {
                var tableName = entityType.Relational().TableName;
                metaTables.Add(new TableMetadata
                {
                    TableName = tableName,
                    AssemblyFullName = entityType.ClrType.FullName,
                    Columns = GetColumnsMetadata(entityType)
                });
                _tableNameLookup.InsertKeyName(tableName);
            }

            return metaTables;
        }

        private IReadOnlyList<ColumnMetadata> GetColumnsMetadata(IEntityType entityType)
        {
            var tableColumns = new List<ColumnMetadata>();
            foreach (var propertyType in entityType.GetProperties())
            {
                var relational = propertyType.Relational();
                tableColumns.Add(new ColumnMetadata
                {
                    ColumnName = relational.ColumnName,
                    DataType = relational.ColumnType
                });
            }
            return tableColumns;
        }
    }
}
