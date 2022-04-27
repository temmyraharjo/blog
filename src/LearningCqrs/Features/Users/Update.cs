using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Core.Handler;
using LearningCqrs.Data;
using MediatR;
using IMapper = AutoMapper.IMapper;
using TimeZoneInfo = LearningCqrs.Data.TimeZoneInfo;

namespace LearningCqrs.Features.Users;

public class Update
{
    public record UpdateUserCommand(string? Password = null,
        [property: Lookup(typeof(TimeZoneInfo))]
        Guid? TimeZoneId = null, string? Email = null,
        string? FirstName = null, string? LastName = null, string? PhoneNumber = null,
        string? PhoneNumber2 = null, string? PhoneNumber3 = null) : IRequest<User>;
    
    public class UpdateUserHandler : UpdateDocumentHandler<UpdateUserCommand, User>
    {
        public UpdateUserHandler(IRepository<User> repository, IMapper mapper, IEnumerable<IValidator<UpdateUserCommand>> validators) :
            base(repository, mapper, validators)
        {
        }
    }
}