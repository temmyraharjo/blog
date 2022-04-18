namespace LearningCqrs.Contracts;

public interface IPost : IEntity
{
    Status Status { get; set; }
    PostType PostType { get; set; }
    string Title { get; set; }
    string Slug { get; set; }
    string Body { get; set; }
    DateTime? CreatedAt { get; set; }
    DateTime? PublishedAt { get; set; }
    IUser? CreatedBy { get; set; }
    IUser? ModifiedBy { get; set; }
    DateTime? ModifiedOn { get; set; }
}