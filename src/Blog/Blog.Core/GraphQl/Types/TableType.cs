using System;
using System.Collections.Generic;
using Blog.Core.GraphQl.Models;
using Blog.Core.GraphQl.Resolvers;
using GraphQL;
using GraphQL.Types;

namespace Blog.Core.GraphQl.Types
{
    public class TableType : ObjectGraphType<object>
    {
        public QueryArguments TableArgs
        {
            get; set;
        }

        private IDictionary<string, Type> _databaseTypeToSystemType;
        protected IDictionary<string, Type> DatabaseTypeToSystemType => 
            _databaseTypeToSystemType ?? 
            (_databaseTypeToSystemType = new Dictionary<string, Type>
        {
            {"uniqueidentifier", typeof(string)},
            {"char", typeof(string)},
            {"nvarchar", typeof(string)},
            {"int", typeof(int)},
            {"decimal", typeof(decimal)},
            {"bit", typeof(bool)}
        });

        public TableType(TableMetadata tableMetadata)
        {
            Name = tableMetadata.TableName;
            foreach (var tableColumn in tableMetadata.Columns)
            {
                InitGraphTableColumn(tableColumn);
            }
        }

        private void InitGraphTableColumn(ColumnMetadata columnMetadata)
        {
            var graphQlType = (ResolveColumnMetaType(columnMetadata.DataType))
                .GetGraphTypeFromType(true);
            var columnField = Field(
                graphQlType,
                columnMetadata.ColumnName
            );

            columnField.Resolver = new NameFieldResolver();
            FillArgs(columnMetadata.ColumnName);
        }

        private void FillArgs(string columnName)
        {
            if (TableArgs == null)
            {
                TableArgs = new QueryArguments(
                    new QueryArgument<StringGraphType>
                    {
                        Name = columnName
                    }
                );
            }
            else
            {
                TableArgs.Add(new QueryArgument<StringGraphType> { Name = columnName });
            }
            TableArgs.Add(new QueryArgument<IdGraphType> { Name = "id" });
            TableArgs.Add(new QueryArgument<IntGraphType> { Name = "first" });
            TableArgs.Add(new QueryArgument<IntGraphType> { Name = "offset" });
        }

        private Type ResolveColumnMetaType(string dbType)
        {
            return DatabaseTypeToSystemType.ContainsKey(dbType ?? "") ? 
                DatabaseTypeToSystemType[dbType] : typeof(string);
        }
    }
}
