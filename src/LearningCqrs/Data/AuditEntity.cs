using System.ComponentModel.DataAnnotations.Schema;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public abstract class AuditEntity : Entity, IAuditEntity
{
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    [ForeignKey("CreatedBy")] public Guid? CreatedById { get; set; }
    [ForeignKey("ModifiedBy")] public Guid? ModifiedById { get; set; }
}