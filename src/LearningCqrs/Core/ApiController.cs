using LearningCqrs.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Core;

public abstract class ApiController : ControllerBase
{
    protected readonly ILogger<ApiController> Logger;

    public ApiController(ILogger<ApiController> logger)
    {
        Logger = logger;
    }
    
    public async Task<ActionResult> Execute<T>(Func<Task<T>> function)
        where T : ActionResult
    {
        var error = "";
        try
        {
            var result = await function.Invoke();
            return result;
        }
        catch (ApiValidationException validationException)
        {
            error = string.Join("\n", validationException.Failures.Select(e => e.PropertyName + ":" + e.ErrorMessage));
            return new BadRequestObjectResult(validationException.Failures);
        }
        catch (Exception ex)
        {
            error = ex.ToString();
            return Problem(ex.Message);
        }
        finally
        {
            if (!string.IsNullOrEmpty(error))
            {
                Logger.LogError(error);
            }
        }
    }
}