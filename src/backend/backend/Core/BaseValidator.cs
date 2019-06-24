using System.Threading.Tasks;
using backend.Data;

namespace backend.Core
{
    public abstract class BaseValidator<TInput>
    {
        public BlogContext Context { get; }

        protected BaseValidator(BlogContext context)
        {
            Context = context;
        }

        public abstract Task<bool> GetResult(TInput input);
    }
}
