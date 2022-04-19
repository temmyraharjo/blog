using LearningCqrs.Contracts;
using LearningCqrs.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningCqrs.Features.Users;

public class GetByUsername
{
    public record GetByUsernameQuery(string Username) : IRequest<User?>;

    public class GetByUsernameHandler : IRequestHandler<GetByUsernameQuery, User?>
    {
        private readonly IRepository<User> _repository;

        public GetByUsernameHandler(IRepository<User> repository)
        {
            _repository = repository;
        }
        
        public async Task<User?> Handle(GetByUsernameQuery request, CancellationToken cancellationToken)
        {
            var filter =
                await _repository.Context.Users.FirstOrDefaultAsync(e => e.Username == request.Username, cancellationToken);

            return filter;
        }
    }
}