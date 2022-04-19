using LearningCqrs.Core;
using LearningCqrs.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace LearningCqrs.Tests.Core;

public class TestContext
{
    private ServiceProvider? _serviceProvider;

    public ServiceProvider ServiceProvider => _serviceProvider ??= GetServiceCollection().BuildServiceProvider();

    private IServiceCollection GetServiceCollection()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddCore();
        serviceCollection.AddTestDatabase();
        return serviceCollection;
    }

    private BlogContext? _blogContext;
    public BlogContext DbContext => _blogContext ??= ServiceProvider.GetService<BlogContext>() ?? throw new NullException(nameof(BlogContext));

    private IMediator? _mediator;

    public IMediator Mediator =>
        _mediator ??= ServiceProvider.GetService<IMediator>() ?? throw new NullException(nameof(IMediator));
}