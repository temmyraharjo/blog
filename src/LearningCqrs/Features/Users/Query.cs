using LearningCqrs.Data;

namespace LearningCqrs.Features.Users;

public class Query
{
    public class QueryUserCommand : Core.Handler.Query.QueryCommand<User, QueryUserResult>
    {
    }

    public class QueryUserResult : Core.Handler.Query.QueryResult<User>
    {
    }

    public class QueryUserHandler : Core.Handler.Query.QueryHandler<User, QueryUserCommand, QueryUserResult>
    {
        public QueryUserHandler(BlogContext context) : base(context)
        {
        }
    }
}