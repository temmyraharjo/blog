using System;
using System.Threading.Tasks;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Core.Handler;
using LearningCqrs.Data;
using LearningCqrs.Features.Categories;
using LearningCqrs.Tests.Core;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

namespace LearningCqrs.Tests.Features.Categories;

public class UpdateTests : BaseUnitTest
{
    public async Task<Guid> GetCategoryId(TestContext testContext)
    {
        var category = new Category { Name = "Hello", Slug = "Hello" };
        await testContext.DbContext.Categories.AddAsync(category);
        await testContext.DbContext.SaveChangesAsync();

        return category.Id;
    }

    [Fact]
    public async Task Category_update_valid()
    {
        var testContext = GetTestContext();

        var categoryId = await GetCategoryId(testContext);

        var jsonPatch = new JsonPatchDocument<Update.UpdateCategoryCommand>();
        jsonPatch.Replace(e => e.Name, "New Title");
        jsonPatch.Add(e => e.UpdateSlug, true);

        var result =
            await testContext.Mediator.Send(
                new UpdateDocument<Update.UpdateCategoryCommand, Category>(categoryId, jsonPatch));

        Assert.Equal(categoryId, result.Id);
        Assert.Equal("New Title", result.Name);
        Assert.Equal("new-title", result.Slug);
    }

    [Fact]
    public async Task Category_update_empty_name()
    {
        var testContext = GetTestContext();

        var categoryId = await GetCategoryId(testContext);

        var jsonPatch = new JsonPatchDocument<Update.UpdateCategoryCommand>();
        jsonPatch.Replace(e => e.Name, "");

        var error = await Assert.ThrowsAsync<ApiValidationException>(() =>
            testContext.Mediator.Send(
                new UpdateDocument<Update.UpdateCategoryCommand, Category>(categoryId, jsonPatch)));

        Assert.Equal("'Name' must not be empty.", error.Failures[0].ErrorMessage);
    }
}