namespace LearningCqrs.Contracts;

public interface IPost : IEntity, IAuditEntity
{
    Status Status { get; set; }
    PostType PostType { get; set; }
    string Title { get; set; }
    string Slug { get; set; }
    string Body { get; set; }
    DateTime? PublishedAt { get; set; }
}