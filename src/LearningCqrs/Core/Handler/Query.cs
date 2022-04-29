using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Swagger;
using LearningCqrs.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Core.Handler;

public class Query
{
    public abstract class QueryResult<TEntity>
        where TEntity : IEntity
    {
        public TEntity[] Data { get; set; }
        public int TotalPage { get; set; }
    }

    public abstract class QueryCommand<TEntity, TOutput> : IRequest<TOutput>
        where TEntity : IEntity
        where TOutput : QueryResult<TEntity>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
        public string? Filter { get; set; }
        public string? Select { get; set; }
        public string? OrderBy { get; set; }

        [SwaggerIgnore]
        public Expression<Func<TEntity, bool>>? FilterExpression => !string.IsNullOrEmpty(Filter)
            ? DynamicExpressionParser.ParseLambda<TEntity, bool>(ParsingConfig.Default, false, Filter, null)
            : null;
    }

    public abstract class QueryHandler<TEntity, TInput, TOutput> : IRequestHandler<TInput, TOutput>
        where TEntity : class, IEntity
        where TInput : QueryCommand<TEntity, TOutput>
        where TOutput : QueryResult<TEntity>
    {
        private readonly BlogContext _context;

        public QueryHandler(BlogContext context)
        {
            _context = context;
        }

        public async Task<TOutput> Handle(TInput request, CancellationToken cancellationToken)
        {
            var dbSet = _context.Set<TEntity>().AsQueryable();
            if (request.FilterExpression != null) dbSet = dbSet.Where(request.FilterExpression);

            if (!string.IsNullOrEmpty(request.OrderBy)) dbSet = dbSet.OrderBy(request.OrderBy);

            if (!string.IsNullOrEmpty(request.Select)) dbSet = dbSet.Select<TEntity>(request.Select);

            var totalData = await dbSet.CountAsync(cancellationToken);
            var totalPage = (int)Math.Ceiling(totalData / (decimal)request.Take);
            var pagingData = await dbSet.Skip(request.Skip).Take(request.Take).ToArrayAsync(cancellationToken);

            var data = Activator.CreateInstance<TOutput>();
            data.TotalPage = totalPage;
            data.Data = pagingData;

            return data;
        }
    }
}