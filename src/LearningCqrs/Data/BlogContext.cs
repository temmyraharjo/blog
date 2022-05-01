using LearningCqrs.Core;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Data;

public class BlogContext : DbContext
{
    private readonly IConfiguration _configuration;

    public BlogContext(DbContextOptions<BlogContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        var connectionString = _configuration.GetConnectionString("BlogConnectionString");
        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TimeZoneInfo> TimeZones { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<PostCategory> PostCategories { get; set; } = null!;
}