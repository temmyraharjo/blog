using System;
using LearningCqrs.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningCqrs.Tests.Core;

public static class ServiceCollectionExtensions
{
    public static void AddTestDatabase(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<BlogContext>(options =>
        {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        });
    }
}