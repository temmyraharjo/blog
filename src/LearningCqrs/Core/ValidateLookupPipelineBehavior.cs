using System.Reflection;
using AutoMapper.Internal;
using FluentValidation.Results;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Data;
using MediatR;

namespace LearningCqrs.Core;

public class ValidateLookupPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly BlogContext _context;
    public ValidateLookupPipelineBehavior(BlogContext context) => _context = context;
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationFailures = new List<ValidationFailure>();
        const BindingFlags bindingFlags = BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;
        var memberList = request.GetType()
            .GetProperties(bindingFlags).Cast<MemberInfo>();

        var checkingList = memberList.Where(m => m.GetCustomAttribute<LookupAttribute>() != null)
            .Select(m => new { MemberInfo = m, LookupAttribute = m.GetCustomAttribute<LookupAttribute>() }).ToArray();
        if (!checkingList.Any()) return await next();

        foreach (var datum in checkingList)
        {
            var attributeName = datum.MemberInfo.Name;
            var value = datum.MemberInfo.GetMemberValue(request);
            if(value == null) continue;
            var ids = (GetArrayGuid(value) ?? new[] { GetSingleGuid(value) ?? Guid.Empty })
                .Where(e=>e!=Guid.Empty).ToArray();
            var entityType = datum.LookupAttribute?.EntityType;
            if(entityType == null) continue;

            foreach (var id in ids)
            {
                var exists = await _context.FindAsync(entityType, id);
                if(exists != null) continue;
                validationFailures.Add(new ValidationFailure(attributeName, $"Entity {entityType.Name} with Id '{id}' does not exists"));
            }
        }

        if (!validationFailures.Any()) return await next();
        throw new ApiValidationException(validationFailures.ToArray());
    }

    private Guid? GetSingleGuid(object value)
    {
        return value as Guid?;
    }

    private Guid[]? GetArrayGuid(Object value)
    {
        return value as Guid[];
    }
}