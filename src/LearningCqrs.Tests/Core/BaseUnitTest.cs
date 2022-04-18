using System;
using System.Linq;
using System.Threading.Tasks;
using LearningCqrs.Core;
using LearningCqrs.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningCqrs.Tests.Core;

public abstract class BaseUnitTest
{
    public ServiceProvider ServiceProvider => GetServiceCollection().BuildServiceProvider();
    private IServiceCollection GetServiceCollection()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddCore();
        serviceCollection.AddCoreDatabase("BlogConnectionString");
        return serviceCollection;
    }

    public BlogContext DbContext => ServiceProvider.GetService<BlogContext>() ?? throw new InvalidOperationException();
    public IMediator Mediator => ServiceProvider.GetService<IMediator>() ?? throw new InvalidOperationException();

    public async Task TruncateTables(params string[] tableNames)
    {
        if(!tableNames.Any()) return;
        
        foreach (var tableName in tableNames)
        {
            var command = $"TRUNCATE TABLE {tableName};";
            await DbContext.Database.ExecuteSqlRawAsync(command);
            Console.WriteLine(command);
        }
    }
}