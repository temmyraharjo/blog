namespace LearningCqrs.Contracts;

public interface IUser : IEntity, IAuditEntity
{
    string Username { get; set; }
    string Password { get; set; }
    Guid? TimeZoneId { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    string? Email { get; set; }
    string? PhoneNumber { get; set; }
    string? PhoneNumber2 { get; set; }
    string? PhoneNumber3 { get; set; }
}