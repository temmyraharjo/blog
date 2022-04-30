using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeZoneInfo = LearningCqrs.Data.TimeZoneInfo;

namespace LearningCqrs.Features.Users;

public class Create
{
    public record CreateUserCommand(string Username, string Password,
        [property: Lookup(typeof(TimeZoneInfo))]
        Guid? TimeZoneId = null, string? Email = null,
        string? FirstName = null, string? LastName = null, string? PhoneNumber = null,
        string? PhoneNumber2 = null, string? PhoneNumber3 = null) : IRequest<DocumentCreated>;

    public class CreateUserHandler : Core.Handler.Create.CreateDocumentHandler<CreateUserCommand>
    {
        private readonly IRepository<User> _repository;
        private readonly IMediator _mediator;

        public CreateUserHandler(IRepository<User> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public override async Task<DocumentCreated> Handling(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                PhoneNumber2 = request.PhoneNumber2,
                PhoneNumber3 = request.PhoneNumber3,
                Email = request.Email
            };
            var password = passwordHasher.HashPassword(user, request.Password);
            user.Password = password;

            var timeZoneId =
                user.TimeZoneId = request.TimeZoneId;
            if (timeZoneId == Guid.Empty)
            {
                var currentTimeZone = await
                    _repository.Context.TimeZones.FirstOrDefaultAsync(e => e.Name == System.TimeZoneInfo.Local.Id,
                        cancellationToken);
                timeZoneId = currentTimeZone?.Id;
            }
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
    }
}