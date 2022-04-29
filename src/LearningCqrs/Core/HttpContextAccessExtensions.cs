using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using MediatR;

namespace LearningCqrs.Core;

public static class HttpContextAccessExtensions
{
    public static async Task<User?> GetCurrentUser(this IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (string.IsNullOrEmpty(currentUser)) return null;

        return await mediator.Send(new GetByUsername.GetByUsernameQuery(currentUser), cancellationToken);
    }
}