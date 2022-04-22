namespace LearningCqrs.Core;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class LookupAttribute : Attribute
{
    public Type EntityType { get; }

    public LookupAttribute(Type entityType)
    {
        EntityType = entityType;
    }
}