using LearningCqrs.Core;
using MediatR;

namespace LearningCqrs.Features.TimeZoneInfo;

public class Create
{
    public record CreateTimezoneCommand(string Name) : IRequest<DocumentCreated>;
    
    public class CreateTimezoneHandler : IRequestHandler<CreateTimezoneCommand, DocumentCreated>
    {
        public Task<DocumentCreated> Handle(CreateTimezoneCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}