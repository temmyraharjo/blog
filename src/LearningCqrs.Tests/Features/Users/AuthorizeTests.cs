using System;
using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using LearningCqrs.Tests.Core;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace LearningCqrs.Tests.Features.Users;

public class AuthorizeTests : BaseUnitTest
{
    [Fact]
    public async void Authorize_correct_password()
    {
        var testContext = GetTestContext();
        var passwordHasher = new PasswordHasher<User>();
       
        var username = "User001";
        var password = "Password";
        var user = new User
        {
            Username = username
        };
        user.Password = passwordHasher.HashPassword(user, password);
        await testContext.DbContext.Users.AddAsync(user);
        await testContext.DbContext.SaveChangesAsync();

        var authorizeCommand = new Authorize.AuthorizeCommand(username, password);
        var resultAuthorize = await testContext.Mediator.Send(authorizeCommand);
        
        Assert.NotNull(resultAuthorize);
    }
    
    [Fact]
    public async void Authorize_bad_password()
    {
        var testContext = GetTestContext();

        var username = "User001";
        var password = "Password";
        await testContext.DbContext.Users.AddAsync(new User{ Username = username, Password = password});
        await testContext.DbContext.SaveChangesAsync();

        var authorizeCommand = new Authorize.AuthorizeCommand(username, $"{password}wrong");
        var result = await Assert.ThrowsAsync<InvalidOperationException>(() => testContext.Mediator.Send(authorizeCommand)) ;
        
        Assert.Equal("Username or Password is incorrect", result.Message);
    }
}