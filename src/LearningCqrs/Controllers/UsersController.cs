using LearningCqrs.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult> Authenticate([FromBody] Features.Users.Authorize.AuthorizeCommand authorizeCommand,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(authorizeCommand, cancellationToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] Features.Users.Create.CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(createUserCommand, cancellationToken);
        return Ok(result);
    }

    [HttpGet("getbyusername")]
    public async Task<ActionResult> GetUserByUsername(
        [FromQuery] Features.Users.GetByUsername.GetByUsernameQuery getByUsernameQuery,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(getByUsernameQuery, cancellationToken);
        return Ok(result);
    }
}