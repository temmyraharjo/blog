using System;
using System.Threading.Tasks;
using LearningCqrs.Core.Exceptions;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using LearningCqrs.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.Users;

public class CreateTests : BaseUnitTest
{
    [Fact]
    public async Task Create_correct_user()
    {
        var testContext = GetTestContext();
        var timeZone = new Data.TimeZoneInfo
        {
            Name = "TZ-001"
        };
        await testContext.DbContext.TimeZones.AddAsync(timeZone);
        await testContext.DbContext.SaveChangesAsync();
        
        var command = new Create.CreateUserCommand("User001", "Password", timeZone.Id);
        var result = await testContext.Mediator.Send(command);
        var userCreated = await testContext.DbContext.Users.SingleAsync(e => e.Id == result.Id);
        Assert.Equal(command.Username, userCreated.Username);
    }

    [Fact]
    public async Task Create_duplicate_username()
    {
        var testContext = GetTestContext();
        var username = "User-001";
        var user = new User
        {
            Username = username,
            Password = "Password"
        };

        await testContext.DbContext.Users.AddAsync(user);
        await testContext.DbContext.SaveChangesAsync();

        var command = new Create.CreateUserCommand(username, "Password", null);
        var error = await Assert.ThrowsAsync<ApiValidationException>(() => testContext.Mediator.Send(command));

        Assert.Equal($"Username '{username}' is already exist.", error.Failures[0].ErrorMessage);
    }

    [Fact]
    public async Task Create_lookup_wrong()
    {
        var testContext = GetTestContext();
        var timezoneId = Guid.NewGuid();
        var command = new Create.CreateUserCommand("User001", "Password", timezoneId);
        var error = await Assert.ThrowsAsync<ApiValidationException>(async () => await testContext.Mediator.Send(command));

        Assert.Equal($"Entity TimeZoneInfo with Id '{timezoneId}' does not exists", error.Failures[0].ErrorMessage);
    }
}