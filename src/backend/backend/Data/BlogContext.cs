using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext() { }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}