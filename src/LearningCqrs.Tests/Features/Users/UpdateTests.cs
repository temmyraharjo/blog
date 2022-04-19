using System;
using System.Threading.Tasks;
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
    [Fact]
    public async Task Update_should_success()
    {
        var testContext = GetTestContext();
        var user = new User { Username = Guid.NewGuid().ToString(), Password = "Password", LastName = "Remove" };
        await testContext.DbContext.Users.AddAsync(user);
        await testContext.DbContext.SaveChangesAsync();

        var updateCommand = new JsonPatchDocument<Update.UpdateUserCommand>();
        updateCommand.Add(e => e.FirstName, "FirstName");
        updateCommand.Replace(e => e.Password, "Password");
        updateCommand.Remove(e => e.LastName);

        await testContext.Mediator.Send(new UpdateDocument<Update.UpdateUserCommand>(user.Id, updateCommand));

        var result = await testContext.DbContext.Users.SingleAsync(e => e.Id == user.Id);
        Assert.Equal("FirstName", result.FirstName);
        Assert.False(result.Password != "Password");
        Assert.Null(result.LastName);
    }
}