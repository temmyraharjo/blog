using LearningCqrs.Core;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UsersController : ApiController
{
    private readonly IMediator _mediator;

    public UsersController(ILogger<ApiController> logger, IMediator mediator) : base(logger)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<ActionResult> Authenticate([FromBody] Authorize.AuthorizeCommand authorizeCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(authorizeCommand, cancellationToken);
            return Ok(result);
        });
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] Create.CreateUserCommand createUserCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(createUserCommand, cancellationToken);
            return Ok(result);
        });
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> PatchUser(Guid id,
        [FromBody] Core.Handler.Update.UpdateDocumentCommand<Update.UpdateUserCommand, User> updateUserCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(
                new Core.Handler.Update.UpdateDocument<Update.UpdateUserCommand, User>(id, updateUserCommand.Patches,
                    updateUserCommand.Version),
                cancellationToken);
            return Ok(result);
        });
    }
    
    [HttpPost("query")]
    public async Task<ActionResult> QueryUsers([FromBody] Query.QueryUserCommand queryPost,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(queryPost, cancellationToken);
            return Ok(result);
        });
    }
}