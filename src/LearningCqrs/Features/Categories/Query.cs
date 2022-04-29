using LearningCqrs.Data;

namespace LearningCqrs.Features.Categories;

public class Query
{
    public class QueryCategoryCommand : Core.Handler.Query.QueryCommand<Category, QueryCategoryResult>
    {
    }

    public class QueryCategoryResult : Core.Handler.Query.QueryResult<Category>
    {
    }

    public class
        QueryCategoryHandler : Core.Handler.Query.QueryHandler<Category, QueryCategoryCommand, QueryCategoryResult>
    {
        public QueryCategoryHandler(BlogContext context) : base(context)
        {
        }
    }
}