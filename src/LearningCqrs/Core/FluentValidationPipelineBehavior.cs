using FluentValidation;
using MediatR;

namespace LearningCqrs.Core;

public class FluentValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public FluentValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
    
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationFailures = _validators.Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null).ToList();

        if (!validationFailures.Any()) return next();

        var error = string.Join("\r\n", validationFailures);
        throw new AggregateException("Model Error: " + error);
    }
}