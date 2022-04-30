using LearningCqrs.Core;
using LearningCqrs.Data;
using LearningCqrs.Features.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/categories")]
[ApiController]
public class CategoriesController : ApiController
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] Create.CreateCategoryCommand createCategoryCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(createCategoryCommand, cancellationToken);
            return Ok(result);
        });
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> PatchCategory(Guid id,
        [FromBody]
        Core.Handler.Update.UpdateDocumentCommand<Update.UpdateCategoryCommand, Category> updateCategoryCommand,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(
                new Core.Handler.Update.UpdateDocument<Update.UpdateCategoryCommand, Category>(id,
                    updateCategoryCommand.Patches,
                    updateCategoryCommand.Version),
                cancellationToken);
            return Ok(result);
        });
    }

    [HttpGet]
    public async Task<ActionResult> GetTimeZones([FromQuery] GetCategory.GetCategoryQuery getCategoryQuery,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(getCategoryQuery, cancellationToken);
            return Ok(result);
        });
    }

    [AllowAnonymous]
    [HttpPost("query")]
    public async Task<ActionResult> Query([FromBody] Query.QueryCategoryCommand queryCategory,
        CancellationToken cancellationToken)
    {
        return await Execute(async () =>
        {
            var result = await _mediator.Send(queryCategory, cancellationToken);
            return Ok(result);
        });
    }
}