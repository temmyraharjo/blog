using System;
using LearningCqrs.Features.Users;
using LearningCqrs.Tests.Core;
using Xunit;

namespace LearningCqrs.Tests.Features.Users;

public class AuthorizeTests : BaseUnitTest
{
    [Fact]
    public async void Authorize_correct_password()
    {
        await TruncateTables("users");

        var username = "User001";
        var password = "Password";
        var command = new Create.CreateUserCommand(username, password);
        await Mediator.Send(command);

        var authorizeCommand = new Authorize.AuthorizeCommand(username, password);
        var resultAuthorize = await Mediator.Send(authorizeCommand);
        
        Assert.NotNull(resultAuthorize);
    }
    
    [Fact]
    public async void Authorize_bad_password()
    {
        await TruncateTables("users");

        var username = "User001";
        var password = "Password";
        var command = new Create.CreateUserCommand(username, password);
        await Mediator.Send(command);

        var authorizeCommand = new Authorize.AuthorizeCommand(username, $"{password}wrong");
        var result = await Assert.ThrowsAsync<InvalidOperationException>(() =>  Mediator.Send(authorizeCommand)) ;
        
        Assert.Equal("Username or Password is incorrect", result.Message);
    }
}