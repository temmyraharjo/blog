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
    public async Task<ActionResult> GetTimeZones([FromQuery] Query.QueryTimeZoneCommand queryTimeZoneCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(queryTimeZoneCommand, cancellationToken);
            return Ok(result);
        });
    }
    
    [HttpPost]
    public async Task<ActionResult> InitTimeZones([FromQuery] InitTimeZone.InitTimeZoneCommand initTimeZoneCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(initTimeZoneCommand, cancellationToken);
            return Ok(result);
        });
    } 
}