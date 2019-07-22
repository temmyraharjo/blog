using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Blog.Core.Core
{
    public class ValidationProcessBehaviour<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse>
    {
        public IEnumerable<IValidator<TRequest>> Validators { get; }

        public ValidationProcessBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            Validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            foreach (var validator in Validators)
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }
            var result = await next();
            return result;
        }
    }
}
