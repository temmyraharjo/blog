using System;
using Blog.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Core
{
    public class BaseTest
    {
        public ITestOutputHelper Output { get; }
        protected ILoggerFactory Logger { get; }

        public BaseTest(ITestOutputHelper output)
        {
            Output = output;
            Logger = new LoggerFactory(new[] { new XunitLoggerProvider(Output) });
        }

        public TestContext GetTestFactory()
        {
            var container = new Container();
            container.Options.AllowOverridingRegistrations = true;

            new Bootstrap(container, true).Handle();

            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(Logger).Options;

            var context = new BlogContext(options);

            container.Register(() => context);

            return new TestContext
            {
                Context = container.GetInstance<BlogContext>(),
                Mediator = container.GetInstance<IMediator>()
            };
        }

        public byte[] GenerateRowVersion()
        {
            return Convert.FromBase64String(Guid.NewGuid().ToString().Replace("-", ""));
        }
    }
}
