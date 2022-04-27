namespace LearningCqrs.Contracts;

public interface IPostCategory : IEntity, IAuditEntity
{
    Guid PostId { get; set; }
    Guid CategoryId { get; set; }
}