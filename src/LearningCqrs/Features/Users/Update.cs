using LearningCqrs.Contracts;
using LearningCqrs.Core.Handler;
using LearningCqrs.Data;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace LearningCqrs.Features.Users;

public class Update
{
    public record UpdateUserCommand(string? Password = null, Guid? TimeZoneId = null, string? Email = null,
        string? FirstName = null, string? LastName = null, string? PhoneNumber = null,
        string? PhoneNumber2 = null, string? PhoneNumber3 = null) : IRequest<Unit>;


    public class UpdateUserHandler : UpdateDocumentHandler<UpdateUserCommand, User>
    {
        public UpdateUserHandler(IRepository<User> repository, IMapper mapper) :
            base(repository, mapper)
        {
        }
    }
}