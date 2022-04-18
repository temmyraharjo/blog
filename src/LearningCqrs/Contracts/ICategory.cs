namespace LearningCqrs.Contracts;

public interface ICategory : IEntity
{
    string Name { get; set; }
    string Slug { get; set; }
}