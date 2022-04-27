using LearningCqrs.Core;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Data;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        var connectionString = Configuration.GetConnectionString("BlogConnectionString");
        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TimeZoneInfo> TimeZones { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<PostCategory> PostCategories { get; set; } = null!;
}