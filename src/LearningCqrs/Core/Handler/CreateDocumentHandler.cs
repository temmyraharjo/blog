using MediatR;

namespace LearningCqrs.Core.Handler;

public abstract class CreateDocumentHandler<TInput>: IRequestHandler<TInput, DocumentCreated>
    where TInput : IRequest<DocumentCreated>
{
    public virtual async Task<DocumentCreated> Handle(TInput request, CancellationToken cancellationToken)
    {
        try
        {
            return await Handling(request, cancellationToken);
        }
        catch (Exception e)
        {
            return new DocumentCreated
            {
                IsSuccess = false,
                ErrorMessage = e.Message
            };
        }
    }

    public abstract Task<DocumentCreated> Handling(TInput request, CancellationToken cancellationToken);
}