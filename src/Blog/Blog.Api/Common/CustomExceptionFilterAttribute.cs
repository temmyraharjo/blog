using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Api.Common
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if (!(context.Exception is ValidationException exception))
                return base.OnExceptionAsync(context);

            context.ExceptionHandled = true;
            var response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";
            context.Result = new JsonResult(exception.Errors);

            return Task.CompletedTask;
        }
    }
}
