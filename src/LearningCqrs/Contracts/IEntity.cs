namespace LearningCqrs.Contracts;

public interface IEntity
{
    Guid Id { get; set; }
    byte[]? RowVersion { get; set; }
}