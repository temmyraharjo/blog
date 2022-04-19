using LearningCqrs.Features.TimeZones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeZoneInfo = LearningCqrs.Data.TimeZoneInfo;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/timezones")]
[ApiController]
public class TimeZonesController : ControllerBase
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
        var result = await _mediator.Send(getTimeZonesQuery, cancellationToken);
        return Ok(result);
    }
}