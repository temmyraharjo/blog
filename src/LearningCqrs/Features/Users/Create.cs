using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LearningCqrs.Features.Users;

public class Create
{
    public record CreateUserCommand(string Username, string Password) : IRequest<DocumentCreated>;
    
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, DocumentCreated>
    {
        private readonly IGenericRepository<User> _repository;

        public CreateUserHandler(IGenericRepository<User> repository)
        {
            _repository = repository;
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