using System.ComponentModel.DataAnnotations;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public class Category : Entity, ICategory
{
    [MaxLength(250)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Slug { get; set; }
}