using Blog.Core.Models;

namespace Blog.Core.Tests.Core
{
    public static class Extensions
    {
        public static async void InitDatabase(this TestContext testContext, Entity[] entities)
        {
            foreach (var entity in entities)
            {
                await testContext.Context.AddAsync(entity);
            }

            await testContext.Context.SaveChangesAsync();

            foreach (var entity in entities)
            {
                testContext.Context.Entry(entity).State = 
                    Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
        }
    }
}
