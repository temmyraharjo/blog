using LearningCqrs.Data;

namespace LearningCqrs.Features.TimeZones;

public class Query
{
    public class QueryTimeZoneCommand : Core.Handler.Query.QueryCommand<Data.TimeZoneInfo, QueryTimeZoneResult>
    {
    }

    public class QueryTimeZoneResult : Core.Handler.Query.QueryResult<Data.TimeZoneInfo>
    {
    }

    public class QueryTimeZoneHandler : Core.Handler.Query.QueryHandler<Data.TimeZoneInfo, QueryTimeZoneCommand, QueryTimeZoneResult>
    {
        public QueryTimeZoneHandler(BlogContext context) : base(context)
        {
        }
    }
}