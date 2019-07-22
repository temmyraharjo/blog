using System.Linq;
using System.Security.Claims;
using Blog.Core.Data;
using GraphQL.Language.AST;
using GraphQL.Validation;

namespace Blog.Core.GraphQl
{
    public class RequiresAuthValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var isAuthenticated = 
                context.UserContext is ClaimsPrincipal userContext 
                && userContext.Identity.IsAuthenticated;
            return new EnterLeaveListener(_ =>
            {
                _.Match<Field>(field =>
                {
                    var tableName = field.Name.Replace("_list", "").ToLower();
                    if(AuthTables.List.Contains(tableName) && !isAuthenticated)
                    {
                        context.ReportError(new ValidationError(
                          context.OriginalQuery,
                          "auth-required",
                          $"Authorization is required to access {field.Name}.",
                          field));
                    }
                });
            });
        }
    }
}
