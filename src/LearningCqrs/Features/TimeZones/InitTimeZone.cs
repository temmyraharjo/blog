using LearningCqrs.Contracts;
using LearningCqrs.Core;
using MediatR;

namespace LearningCqrs.Features.TimeZones;

public class InitTimeZone
{
    public record InitTimeZoneCommand() : IRequest<Unit>;
    
    public class InitTimeZoneHandler : IRequestHandler<InitTimeZoneCommand, Unit>
    {
        private readonly IRepository<Data.TimeZoneInfo> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public InitTimeZoneHandler(IRepository<Data.TimeZoneInfo> repository, 
            IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(InitTimeZoneCommand request, CancellationToken cancellationToken)
        {
            var timeZones = System.TimeZoneInfo.GetSystemTimeZones();
            var entities = timeZones.Select(tz => new Data.TimeZoneInfo
            {
                Name = tz.Id
            }).ToArray();
            if(!entities.Any()) return Unit.Value;

            var timeZonesWithAuditProperty = (Data.TimeZoneInfo[]) await entities.SetAuditProperty(_httpContextAccessor, _mediator, cancellationToken);
            await _repository.Context.TimeZones.AddRangeAsync(timeZonesWithAuditProperty, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}