using backend.Features.User;
using Xunit;

namespace backend.tests.Features.User
{
    public class UserNameUniqueValidatorTests : BaseTest
    {
        [Fact]
        public async void UserUniqueValidator_ShouldThrow()
        {
            var context = GetContext();
            var user = new Data.User
            {
                UserName = "user-001",
                FirstName = "First Name",
                LastName = "Last Name"
            };
            context.Users.Add(user);
            context.SaveChanges();

            var isExists = await new UserNameUniqueValidator(context).GetResult("user-001");

            Assert.True(isExists);
        }

        [Fact]
        public async void UserUniqueValidator_Valid()
        {
            var context = GetContext();
            var user = new Data.User
            {
                UserName = "user-002",
                FirstName = "First Name",
                LastName = "Last Name"
            };
            context.Users.Add(user);
            context.SaveChanges();

            var isExists = await new UserNameUniqueValidator(context).GetResult("user-001");

            Assert.False(isExists);
        }
    }
}
