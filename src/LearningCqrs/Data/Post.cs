using System.ComponentModel.DataAnnotations;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public class Post : AuditEntity, IPost
{
    public Status Status { get; set; }
    public PostType PostType { get; set; }
    [MaxLength(1000)] public string? Title { get; set; }
    [MaxLength(1000)] public string? Slug { get; set; }
    public string? Body { get; set; }
    public DateTime? PublishedAt { get; set; }
    public ICollection<PostCategory>? PostCategories { get; set; }
}