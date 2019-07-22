using Blog.Core.Data;
using Blog.Core.GraphQl.Models;
using Blog.Core.GraphQl.Resolvers;
using Blog.Core.GraphQl.Types;
using GraphQL.Types;

namespace Blog.Core.GraphQl
{
    public sealed class GraphQlQuery : ObjectGraphType<object>
    {
        public GraphQlQuery(
            BlogContext dbContext,
            IDatabaseMetadata dbMetadata,
            ITableNameLookup tableNameLookup)
        {
            var dbContext1 = dbContext;
            Name = "Query";
            foreach (var metaTable in dbMetadata.GetTableMetadatas())
            {
                var tableType = new TableType(metaTable);
                var friendlyTableName = tableNameLookup.
                    GetFriendlyName(metaTable.TableName);
                AddField(new FieldType
                {
                    Name = friendlyTableName,
                    Type = tableType.GetType(),
                    ResolvedType = tableType,
                    Resolver = new FieldResolver(metaTable, dbContext1),
                    Arguments = new QueryArguments(
                        tableType.TableArgs
                    )
                });
                // lets add key to get list of current table
                var listType = new ListGraphType(tableType);
                AddField(new FieldType
                {
                    Name = $"{friendlyTableName}_list",
                    Type = listType.GetType(),
                    ResolvedType = listType,
                    Resolver = new FieldResolver(metaTable, dbContext1),
                    Arguments = new QueryArguments(
                        tableType.TableArgs
                    )
                });
            }
        }
    }
}
