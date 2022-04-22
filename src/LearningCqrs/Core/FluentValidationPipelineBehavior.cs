using System.Reflection;
using AutoMapper.Internal;
using FluentValidation;
using FluentValidation.Results;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TimeZoneInfo = LearningCqrs.Data.TimeZoneInfo;

namespace LearningCqrs.Core;

public class FluentValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly BlogContext _context;

    public FluentValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators, BlogContext context)
    {
        _validators = validators;
        _context = context;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var validationFailures = _validators.Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null).ToList();

        if (validationFailures.Any()) throw new ApiValidationException(validationFailures.ToArray());

        const BindingFlags bindingFlags = BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;
        var memberList = request.GetType()
            .GetProperties(bindingFlags).Cast<MemberInfo>();

        var checkingList = memberList.Where(m => m.GetCustomAttribute<LookupAttribute>() != null)
            .Select(m => new { MemberInfo = m, LookupAttribute = m.GetCustomAttribute<LookupAttribute>() }).ToArray();
        if (!checkingList.Any()) return next();

        validationFailures.AddRange(from datum in checkingList
            let attributeName = datum.MemberInfo.Name
            let id = datum.MemberInfo.GetMemberValue(request) as Guid?
            let entityType = datum.LookupAttribute?.EntityType
            let valid = id != null && entityType != null
            where valid
            let exists = _context.Find(entityType, id)
            where exists == null
            select new ValidationFailure(attributeName, $"Entity {entityType.Name} with Id '{id}' does not exists"));

        if (!validationFailures.Any()) return next();
        throw new ApiValidationException(validationFailures.ToArray());
    }
}