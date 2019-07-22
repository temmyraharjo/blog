using System;
using System.Linq.Dynamic.Core;
using Blog.Core.Data;
using Blog.Core.GraphQl.Models;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace Blog.Core.GraphQl.Resolvers
{
    public class FieldResolver : IFieldResolver
    {
        private readonly TableMetadata _tableMetadata;
        private readonly BlogContext _dbContext;

        public FieldResolver(TableMetadata tableMetadata, BlogContext dbContext)
        {
            _tableMetadata = tableMetadata;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public object Resolve(ResolveFieldContext context)
        {
            var queryable = _dbContext.Query(_tableMetadata.AssemblyFullName);
            if (context.FieldName.Contains("_list"))
            {

                var first = context.Arguments["first"] != null ?
                    context.GetArgument("first", int.MaxValue) :
                    int.MaxValue;
                var offset = context.Arguments["offset"] != null ?
                    context.GetArgument("offset", 0) :
                    0;
                return queryable
                    .Skip(offset)
                    .Take(first)
                    .ToDynamicList<object>();
            }

            var id = context.GetArgument<long>("id");
            return queryable.FirstOrDefault("Id == @0", id);
        }
    }
}
