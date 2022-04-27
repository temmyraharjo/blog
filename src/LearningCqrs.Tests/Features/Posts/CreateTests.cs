using System;
using System.Linq;
using System.Threading.Tasks;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Data;
using LearningCqrs.Features.Posts;
using LearningCqrs.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.Posts;

public class CreateTests : BaseUnitTest
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

    [Fact]
    public async Task Post_create_valid()
    {
        var testContext = GetTestContext();
        var categoryIds = await GetCategoryIds(testContext);

        var command = new Create.CreatePostCommand(Title: "Title", Status: Status.Draft,
            CategoryIds: categoryIds, PostType: PostType.Blog, PublishedAt: DateTime.Now, Body: "This is body");
        var result = await testContext.Mediator.Send(command);

        var createdPost = await testContext.DbContext.Posts.SingleAsync(e => e.Id == result.Id);
        Assert.Equal("Title", createdPost.Title);
        Assert.Equal("title", createdPost.Slug);
        Assert.Equal(Status.Draft, createdPost.Status);
        Assert.Equal(PostType.Blog, createdPost.PostType);
        Assert.Equal("This is body", createdPost.Body);
        Assert.NotNull(createdPost.PublishedAt);
        var createdCategories = createdPost.PostCategories?.ToArray();
        Assert.Equal(categoryIds[0], createdCategories![0].CategoryId);
        Assert.Equal(categoryIds[1], createdCategories[1].CategoryId);
    }

    [Fact]
    public async Task Post_create_status_validation()
    {
        var testContext = GetTestContext();
        var categoryIds = await GetCategoryIds(testContext);

        var command = new Create.CreatePostCommand(Title: "Title", Status: Status.Published,
            CategoryIds: categoryIds, PostType: PostType.Blog, Body: "This is body");
        var error = await Assert.ThrowsAsync<ApiValidationException>(() => testContext.Mediator.Send(command));

        Assert.Equal("'Published At' must not be empty.", error.Failures[0].ErrorMessage);
    }
}