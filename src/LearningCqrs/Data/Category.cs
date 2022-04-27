using System.ComponentModel.DataAnnotations;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public class Category : AuditEntity, ICategory
{
    [MaxLength(250)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string Slug { get; set; } = null!;
    public ICollection<PostCategory>? PostCategories { get; set; }
}