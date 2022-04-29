using LearningCqrs.Data;

namespace LearningCqrs.Features.Posts;

public class Query
{
    public class QueryPostCommand : Core.Handler.Query.QueryCommand<Post, QueryPostResult>
    {
    }

    public class QueryPostResult : Core.Handler.Query.QueryResult<Post>
    {
    }

    public class QueryPostHandler : Core.Handler.Query.QueryHandler<Post, QueryPostCommand, QueryPostResult>
    {
        public QueryPostHandler(BlogContext context) : base(context)
        {
        }
    }
}