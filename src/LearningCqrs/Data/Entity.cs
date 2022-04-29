using System.ComponentModel.DataAnnotations;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public abstract class Entity : IEntity
{
    [Key] public Guid Id { get; set; }

    [Timestamp] public byte[]? RowVersion { get; set; }
}