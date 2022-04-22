using LearningCqrs.Core;
using LearningCqrs.Features.TimeZones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/timezones")]
[ApiController]
public class TimeZonesController : ApiController
{
    private readonly IMediator _mediator;

    public TimeZonesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetTimeZones([FromQuery] GetTimeZones.GetTimeZonesQuery getTimeZonesQuery,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(getTimeZonesQuery, cancellationToken);
            return Ok(result);
        });
    }
}