namespace LearningCqrs.Core;

public class DocumentCreated
{
    public string EntityLogicalName { get; set; } = null!;
    public Guid? Id { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string? ErrorMessage { get; set; }
}