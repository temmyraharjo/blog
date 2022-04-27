using System.ComponentModel.DataAnnotations.Schema;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public class PostCategory : AuditEntity, IPostCategory
{
    [ForeignKey("Post")] public Guid PostId { get; set; }
    public Post? Post { get; set; }
    [ForeignKey("Category")] public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}