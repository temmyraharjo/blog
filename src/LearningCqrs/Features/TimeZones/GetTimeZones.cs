using LearningCqrs.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Features.TimeZones;

public class GetTimeZones
{
    public record GetTimeZonesQuery(Guid? TimeZoneId, string? Contains, string? Equal) : IRequest<Data.TimeZoneInfo[]>;

    public class GetTimeZonesHandler : IRequestHandler<GetTimeZonesQuery, Data.TimeZoneInfo[]>
    {
        private readonly IRepository<Data.TimeZoneInfo> _repository;

        public GetTimeZonesHandler(IRepository<Data.TimeZoneInfo> repository)
        {
            _repository = repository;
        }

        public async Task<Data.TimeZoneInfo[]> Handle(GetTimeZonesQuery request, CancellationToken cancellationToken)
        {
            try
            {

            }
            catch
            {
                return new Data.TimeZoneInfo[] { };
            }
            if (request.TimeZoneId.HasValue)
            {
                var result = await _repository.Context.TimeZones
                    .FirstOrDefaultAsync(e => e.Id == request.TimeZoneId, cancellationToken);
                if (result != null) return new[] { result };
            }

            if (!string.IsNullOrEmpty(request.Contains))
            {
                var result = await _repository.Context.TimeZones
                    .Where(e => e.Name.Contains(request.Contains))
                    .ToArrayAsync(cancellationToken);
                return result;
            }

            if (!string.IsNullOrEmpty(request.Equal))
            {
                var result =
                    await _repository.Context.TimeZones.FirstOrDefaultAsync(e => e.Name == request.Equal,
                        cancellationToken);
                if (result != null) return new[] { result };
            }

            return await _repository.Context.TimeZones.ToArrayAsync(cancellationToken);
        }
    }
}