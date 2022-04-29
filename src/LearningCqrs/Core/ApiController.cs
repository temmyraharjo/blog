using LearningCqrs.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Core;

public abstract class ApiController : ControllerBase
{
    public async Task<ActionResult> Execute<T>(Func<Task<T>> function)
        where T : ActionResult
    {
        try
        {
            var result = await function.Invoke();
            return result;
        }
        catch (ApiValidationException validationException)
        {
            return new BadRequestObjectResult(validationException.Failures);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}