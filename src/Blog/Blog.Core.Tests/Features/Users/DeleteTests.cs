using System;
using Blog.Core.Features.User;
using Blog.Core.Models;
using Blog.Core.Tests.Core;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Features.Users
{
    public class DeleteTests : BaseTest
    {
        public DeleteTests(ITestOutputHelper output) :
            base(output)
        {
        }

        [Fact]
        public async void User_delete_should_valid()
        {
            var factory = GetTestFactory();

            const int id = 1;
            var rowVersion = GenerateRowVersion();

            factory.InitDatabase(new Entity[]{
                new User
                {
                    Id = id,
                    UserName = "user001",
                    RowVersion = rowVersion
                }
            });

            var command = new Delete.Command { Id = id, RowVersion = rowVersion };
            var result = await factory.Mediator.Send(command);

            Assert.True(result);
        }

        [Fact]
        public async void User_delete_should_not_valid()
        {
            var factory = GetTestFactory();

            const int id = 1;
            var rowVersion = GenerateRowVersion();

            factory.InitDatabase(new Entity[]{
                new User
                {
                    Id = id,
                    UserName = "user001",
                    RowVersion = rowVersion
                }
            });

            var command = new Delete.Command { Id = id, RowVersion = GenerateRowVersion() };

            var error = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => factory.Mediator.Send(command));
            Assert.NotNull(error);
        }
    }
}
