using LearningCqrs.Core.Handler;
using LearningCqrs.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    public async Task<ActionResult> Authenticate([FromBody] Authorize.AuthorizeCommand authorizeCommand,
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
    public async Task<ActionResult> CreateUser([FromBody] Create.CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(createUserCommand, cancellationToken);
        return Ok(result);
    }

    [HttpGet("getbyusername")]
    public async Task<ActionResult> GetUserByUsername(
        [FromQuery] GetByUsername.GetByUsernameQuery getByUsernameQuery,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(getByUsernameQuery, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> PatchUser(Guid id,
        [FromBody] JsonPatchDocument<Update.UpdateUserCommand> updateUserCommand,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateDocument<Update.UpdateUserCommand>(id, updateUserCommand),
            cancellationToken));
    }
}