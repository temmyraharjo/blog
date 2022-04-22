using System.Threading.Tasks;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Data;
using LearningCqrs.Features.Categories;
using LearningCqrs.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.Categories;

public class CreateTests : BaseUnitTest
{
    [Fact]
    public async Task Category_create_valid()
    {
        var testContext = GetTestContext();

        var command = new Create.CreateCategoryCommand("Category001");
        await testContext.Mediator.Send(command);

        var createdCategory = await testContext.DbContext.Categories.SingleAsync();
        Assert.Equal("Category001", createdCategory.Name);
        Assert.NotEmpty(createdCategory.Slug);
    }

    [Fact]
    public async Task Category_create_duplicate()
    {
        var testContext = GetTestContext();

        await testContext.DbContext.Categories.AddAsync(new Category
        {
            Name = "Category001",
            Slug = "Category001"
        });
        await testContext.DbContext.SaveChangesAsync();

        var command = new Create.CreateCategoryCommand("Category001");
        var error = await Assert.ThrowsAsync<ApiValidationException>(() => testContext.Mediator.Send(command));
        Assert.Equal($"Category 'Category001' is already exist.", error.Failures[0].ErrorMessage);
    }
}