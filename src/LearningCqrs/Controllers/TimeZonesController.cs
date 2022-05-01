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

    public TimeZonesController(ILogger<ApiController> logger, IMediator mediator) : base(logger)
    {
        _mediator = mediator;
    }

    [HttpPost("query")]
    public async Task<ActionResult> QueryTimeZones([FromBody] Query.QueryTimeZoneCommand queryTimeZoneCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(queryTimeZoneCommand, cancellationToken);
            return Ok(result);
        });
    }
    
    [HttpPost]
    public async Task<ActionResult> InitTimeZones([FromBody] InitTimeZone.InitTimeZoneCommand initTimeZoneCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            await _mediator.Send(initTimeZoneCommand, cancellationToken);
            return Ok("Created");
        });
    } 
}