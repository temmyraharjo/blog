namespace LearningCqrs.Contracts;

public interface IAuditEntity
{
    DateTime? CreatedOn { get; set; }
    DateTime? ModifiedOn { get; set; }

    Guid? CreatedById { get; set; }
    Guid? ModifiedById { get; set; }
}