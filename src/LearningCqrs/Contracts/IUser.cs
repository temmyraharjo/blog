namespace LearningCqrs.Contracts;

public interface IUser : IEntity
{
    string Username { get; set; }
    string Password { get; set; }
    Guid? TimeZoneId { get; set; }
}