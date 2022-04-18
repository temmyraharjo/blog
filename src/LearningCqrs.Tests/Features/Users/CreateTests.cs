using System;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using LearningCqrs.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LearningCqrs.Tests.Features.Users;

public class CreateTests : BaseUnitTest
{
    [Fact]
    public async void Create_correct_user()
    {
        await TruncateTables("users");

        var command = new Create.CreateUserCommand("User001", "Password");
        var result = await Mediator.Send(command);
        var userCreated = await DbContext.Users.SingleAsync(e => e.Id == result.Id);
        Assert.Equal(command.Username, userCreated.Username);
    }

    [Fact]
    public async void Create_duplicate_username()
    {
        var dbContext = DbContext;
        await TruncateTables("users");

        var username = $"User-001";
        var user = new User
        {
            Username = username,
            Password = "Password"
        };
        
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var command = new Create.CreateUserCommand(username, "Password");
        var result = await Assert.ThrowsAsync<AggregateException>(() => Mediator.Send(command));
        
        Assert.Equal($"Model Error: Username '{username}' is already exist.", result.Message);
    }
}