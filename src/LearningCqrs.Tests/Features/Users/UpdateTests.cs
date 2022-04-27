using System;
using System.Threading.Tasks;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Core.Handler;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using LearningCqrs.Tests.Core;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.Users;

public class UpdateTests : BaseUnitTest
{
    public async Task<Guid> GetUserId(TestContext testContext)
    {
        var user = new User { Username = Guid.NewGuid().ToString(), Password = "Password", LastName = "Remove" };
        await testContext.DbContext.Users.AddAsync(user);
        await testContext.DbContext.SaveChangesAsync();

        return user.Id;
    }

    [Fact]
    public async Task Update_should_success()
    {
        var testContext = GetTestContext();
        var userId = await GetUserId(testContext);

        var updateCommand = new JsonPatchDocument<Update.UpdateUserCommand>();
        updateCommand.Add(e => e.FirstName, "FirstName");
        updateCommand.Replace(e => e.Password, "Password");
        updateCommand.Remove(e => e.LastName);

        await testContext.Mediator.Send(new UpdateDocument<Update.UpdateUserCommand, User>(userId, updateCommand));

        var result = await testContext.DbContext.Users.SingleAsync(e => e.Id == userId);
        Assert.Equal("FirstName", result.FirstName);
        Assert.False(result.Password != "Password");
        Assert.Null(result.LastName);
    }

    [Fact]
    public async Task Update_should_error_password_empty()
    {
        var testContext = GetTestContext();
        var userId = await GetUserId(testContext);

        var updateCommand = new JsonPatchDocument<Update.UpdateUserCommand>();
        updateCommand.Add(e => e.FirstName, "FirstName");
        updateCommand.Replace(e => e.Password, "");
        updateCommand.Remove(e => e.LastName);

        var error = await Assert.ThrowsAsync<ApiValidationException>(() =>
            testContext.Mediator.Send(new UpdateDocument<Update.UpdateUserCommand, User>(userId, updateCommand)));

        Assert.Equal("'Password' must not be empty.", error.Failures[0].ErrorMessage);
    }
}