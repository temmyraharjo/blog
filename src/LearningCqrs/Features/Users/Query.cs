using LearningCqrs.Data;
using MediatR;

namespace LearningCqrs.Features.Users;

public class Query
{
    public class QueryCommand : Core.Handler.Query.QueryCommand<User, QueryResult>
    {
    }

    public class QueryResult : Core.Handler.Query.QueryResult<User>
    {
    }

    public class QueryHandler : Core.Handler.Query.QueryHandler<User, QueryCommand, QueryResult>
    {
        public QueryHandler(BlogContext context) : base(context)
        {
        }
    }
}