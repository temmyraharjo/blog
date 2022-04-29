using System;
using System.Linq;
using System.Threading.Tasks;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Core.Handler;
using LearningCqrs.Data;
using LearningCqrs.Tests.Core;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;

namespace LearningCqrs.Tests.Features.Posts;

public class UpdateTests : BaseUnitTest
{
    public async Task<Guid[]> GetCategoryIds(TestContext testContext)
    {
        var category1 = new Category { Name = "Category001", Slug = "category001" };
        await testContext.DbContext.Categories.AddAsync(category1);

        var category2 = new Category { Name = "Category002", Slug = "category002" };
        await testContext.DbContext.Categories.AddAsync(category2);

        await testContext.DbContext.SaveChangesAsync();

        return new[] { category1.Id, category2.Id };
    }

    public async Task<Post> GetPost(TestContext testContext)
    {
        var post = new Post
        {
            Title = "test",
            Body = "test",
            Slug = "test"
        };
        await testContext.DbContext.Posts.AddAsync(post);
        await testContext.DbContext.SaveChangesAsync();

        return post;
    }

    [Fact]
    public async Task Post_update_valid()
    {
        var testContext = GetTestContext();
        var categoryIds = await GetCategoryIds(testContext);
        var post = await GetPost(testContext);

        var jsonPatch = new JsonPatchDocument<LearningCqrs.Features.Posts.Update.UpdatePostCommand>();
        jsonPatch.Replace(e => e.Title, "New Title");
        jsonPatch.Add(e => e.UpdateSlug, true);
        jsonPatch.Replace(e => e.Body, "New Body");
        jsonPatch.Add(e => e.CategoryIds, categoryIds);

        var result = await testContext.Mediator.Send(
            new Update.UpdateDocument<LearningCqrs.Features.Posts.Update.UpdatePostCommand, Post>(post.Id, jsonPatch));

        Assert.Equal("New Title", result.Title);
        Assert.Equal("new-title", result.Slug);
        Assert.Equal("New Body", result.Body);
        var postCategories = result.PostCategories!.ToArray();
        Assert.Equal(categoryIds[0], postCategories[0].CategoryId);
        Assert.Equal(categoryIds[1], postCategories[1].CategoryId);
    }

    [Fact]
    public async Task Post_update_validation_error()
    {
        var testContext = GetTestContext();
        var categoryIds = await GetCategoryIds(testContext);
        var post = await GetPost(testContext);

        var jsonPatch = new JsonPatchDocument<LearningCqrs.Features.Posts.Update.UpdatePostCommand>();
        jsonPatch.Replace(e => e.Title, "New Title");
        jsonPatch.Add(e => e.UpdateSlug, true);
        jsonPatch.Replace(e => e.Body, "New Body");
        jsonPatch.Add(e => e.CategoryIds, categoryIds);
        jsonPatch.Add(e => e.Status, Status.Published);

        var error = await Assert.ThrowsAsync<ApiValidationException>(() => testContext.Mediator.Send(
            new Update.UpdateDocument<LearningCqrs.Features.Posts.Update.UpdatePostCommand, Post>(post.Id, jsonPatch)));

        Assert.Equal("'Published At' must not be empty.", error.Failures[0].ErrorMessage);
    }
}