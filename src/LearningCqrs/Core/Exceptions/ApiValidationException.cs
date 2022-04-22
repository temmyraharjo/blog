using FluentValidation.Results;

namespace LearningCqrs.Core.Exceptions;

public class ApiValidationException : Exception
{
    public  ValidationFailure[] Failures { get; }

    public ApiValidationException(ValidationFailure[] failures)
    {
        Failures = failures;
    }
}