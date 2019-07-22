using System;
using System.Linq;
using Blog.Core.Features.User;
using Blog.Core.Models;
using Blog.Core.Tests.Core;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Core.Tests.Features.Users
{
    public class UpdateTests : BaseTest
    {
        public UpdateTests(ITestOutputHelper output) : 
            base(output)
        {
        }

        [Fact]
        public async void User_update_should_valid()
        {
            var factory = GetTestFactory();

            const int id = 1;
            var rowVersion = GenerateRowVersion();

            factory.InitDatabase(new Entity[]
            {
                new User
                {
                    Id = id,
                    RowVersion = rowVersion,
                    FirstName = "User",
                    LastName = "001",
                    UserName = "user001"
                }
            });
            
            var command = new Update.Command
            {
                Id = id,
                FirstName = "User",
                LastName = "001",
                Operations = new []
                {
                    new Microsoft.AspNetCore.JsonPatch.Operations.Operation
                    {
                        op = "replace", path = "/firstName", value = "Test"
                    },
                    new Microsoft.AspNetCore.JsonPatch.Operations.Operation
                    {
                        op = "replace", path = "/lastName", value = "002"
                    }
                }.ToList(),
                RowVersion = rowVersion
            };

            var result = await factory.Mediator.Send(command);

            Assert.True(result);
        }
    }
}
