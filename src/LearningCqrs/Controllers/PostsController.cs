using LearningCqrs.Core;
using LearningCqrs.Data;
using LearningCqrs.Features.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/posts")]
[ApiController]
public class PostsController : ApiController
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreatePost([FromBody] Create.CreatePostCommand createPostCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(createPostCommand, cancellationToken);
            return Ok(result);
        });
    }

    [HttpPatch]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> PatchPost(Guid id,
        [FromBody] Core.Handler.Update.UpdateDocumentCommand<Update.UpdatePostCommand, Post> updatePostCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(
                new Core.Handler.Update.UpdateDocument<Update.UpdatePostCommand, Post>(id, updatePostCommand.Patches,
                    updatePostCommand.Version),
                cancellationToken);
            return Ok(result);
        });
    }

    [AllowAnonymous]
    [HttpPost("query")]
    public async Task<ActionResult> Query([FromBody] Query.QueryPostCommand queryPost,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(queryPost, cancellationToken);
            return Ok(result);
        });
    }
}