using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

[Table("Timezones")]
public class TimeZoneInfo : AuditEntity, ITimeZoneInfo
{
    [MaxLength(100)] public string Name { get; set; } = null!;
}