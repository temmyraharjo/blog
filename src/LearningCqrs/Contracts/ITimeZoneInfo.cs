namespace LearningCqrs.Contracts;

public interface ITimeZoneInfo : IEntity, IAuditEntity
{
    string Name { get; set; }
}