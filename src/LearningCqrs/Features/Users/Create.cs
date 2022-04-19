using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Data;
using LearningCqrs.Features.TimeZones;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningCqrs.Features.Users;

public class Create
{
    public record CreateUserCommand(string Username, string Password, Guid? TimeZoneId) : IRequest<DocumentCreated>;
    
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, DocumentCreated>
    {
        private readonly IRepository<User> _repository;
        private readonly IMediator _mediator;

        public CreateUserHandler(IRepository<User> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        
        public async Task<DocumentCreated> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var passwordHasher = new PasswordHasher<User>();
                var user = new User();
                var password = passwordHasher.HashPassword(user, request.Password);
                user.Password = password;
                user.Username = request.Username;

                var timeZoneId =
                    user.TimeZoneId = request.TimeZoneId ?? (await _mediator.Send(
                        new GetTimeZones.GetTimeZonesQuery(null, null, System.TimeZoneInfo.Local.Id),
                        cancellationToken)).FirstOrDefault()?.Id;
                user.TimeZoneId = timeZoneId;

                await _repository.CreateAsync(user, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);

                return new DocumentCreated
                {
                    Id = user.Id,
                    IsSuccess = true,
                    EntityLogicalName = nameof(User)
                };
            }
            catch (Exception e)
            {
                return new DocumentCreated
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                    EntityLogicalName = nameof(User)
                };
            }
        }
    }
}