using LearningCqrs.Data;
using LearningCqrs.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningCqrs.Core;

public static class ControllerExtensions
{
    public static async Task<User?> GetCurrentUser(this ControllerBase controller, IMediator mediator, CancellationToken cancellationToken)
    {
        var userName = controller.User?.Identity?.Name;
        if (string.IsNullOrEmpty(userName)) return null;

        return await mediator.Send(new GetByUsername.GetByUsernameQuery(userName),
            cancellationToken);
    }
}