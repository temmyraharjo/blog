using System;
using System.Threading.Tasks;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using LearningCqrs.Tests.Core;
using Xunit;

namespace LearningCqrs.Tests.Features.Users;

public class QueryTests : BaseUnitTest
{
    private async Task InitData(TestContext testContext)
    {
        var users = new[]
        {
            new User { Id = Guid.NewGuid(), FirstName = "User 001", LastName = "Test" },
            new User { Id = Guid.NewGuid(), FirstName = "User 002", LastName = "Test" },
            new User { Id = Guid.NewGuid(), FirstName = "User 003", LastName = "Test" }
        };

        await testContext.DbContext.Users.AddRangeAsync(users);
        await testContext.DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Query_should_valid()
    {
        var testContext = GetTestContext();
        await InitData(testContext);

        var query = new Query.QueryUserCommand
        {
            Skip = 0,
            Take = 2,
            Filter = "FirstName.Contains(\"User 00\")",
            OrderBy = "FirstName DESC",
            Select = "new { FirstName }"
        };

        var result = await testContext.Mediator.Send(query);
        Assert.Equal(2, result.TotalPage);
        Assert.Equal(2, result.Data.Length);
        Assert.Equal("User 003", result.Data[0].FirstName);
        Assert.Equal("User 002", result.Data[1].FirstName);
    }
}