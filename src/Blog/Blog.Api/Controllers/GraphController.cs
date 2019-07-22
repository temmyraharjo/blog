using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.GraphQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        public Schema Schema { get; }

        public GraphController(Schema schema)
        {
            Schema = schema;
        }

        [HttpPost]
        public async Task<ActionResult> Get([FromQuery] string query, 
            CancellationToken cancellationToken)
        {
            var result = await new DocumentExecuter().ExecuteAsync(
                new ExecutionOptions
                {
                    Schema = Schema,
                    CancellationToken = cancellationToken,
                    Query = query,
                    UserContext = User,
                    ValidationRules = new[]
                    {
                        new RequiresAuthValidationRule()
                    }
                }); 

            if (result.Errors != null && result.Errors.Any())
            {
                return Ok(result.Errors);
            }

            return Ok(result.Data);
        }
    }
}