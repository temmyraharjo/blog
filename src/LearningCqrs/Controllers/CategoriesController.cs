using LearningCqrs.Features.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Controllers;

[Authorize]
[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody]Create.CreateCategoryCommand createCategoryCommand,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(createCategoryCommand, cancellationToken);
        return Ok(result);
    }
}