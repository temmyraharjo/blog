using Blog.Core.Features.User;
using Blog.Core.Tests.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Features.Users
{
    public class CreateTests : BaseTest
    {
        public CreateTests(ITestOutputHelper output) : 
            base(output)
        {
        }

        [Fact]
        public async void Create_user_should_valid()
        {
            var factory = GetTestFactory();

            var command = new Create.Command
            {
                FirstName = "user",
                LastName = "001",
                UserName = "user-001",
                Password = "password",
                Repassword = "password"
            };

            var userId = await factory.Mediator.Send(command);
            var user = await factory.Context.Users.
                FirstOrDefaultAsync(e => e.UserName == "user-001");
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async void Create_user_not_valid_username()
        {
            var factory = GetTestFactory();

            factory.InitDatabase(new[]{ new Models.User
            {
                UserName = "user-001"
            }});

            var command = new Create.Command
            {
                FirstName = "user",
                LastName = "001",
                UserName = "user-001",
                Password = "password",
                Repassword = "password"
            };

            var error = await Assert.ThrowsAsync<ValidationException>(() => 
                factory.Mediator.Send(command));
            Assert.NotNull(error.Errors);
        }
    }
}
