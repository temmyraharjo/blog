using Blog.Core.Data;
using MediatR;

namespace Blog.Core.Tests.Core
{
    public class TestContext
    {
        public IMediator Mediator { get; set; }
        public BlogContext Context { get; set; }
    }
}
