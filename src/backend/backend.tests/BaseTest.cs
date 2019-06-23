using System;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.tests
{
    public abstract class BaseTest
    {
        protected BlogContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new BlogContext(options);
        }
    }
}