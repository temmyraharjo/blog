using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearningCqrs.Contracts;
using LearningCqrs.Core;
using LearningCqrs.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LearningCqrs.Features.Users;

public class Authorize
{
    public record AuthorizeCommand(string Username, string Password) : IRequest<string>;

    public class AuthorizeHandler : IRequestHandler<AuthorizeCommand, string>
    {
        private readonly IRepository<User> _repository;
        private readonly IConfiguration _configuration;

        public AuthorizeHandler(IRepository<User> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.Context.Users.FirstOrDefaultAsync(e => e.Username == request.Username,
                cancellationToken);
            if (user == null) throw new InvalidOperationException("Username does not exists");
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new InvalidOperationException("Username or Password is incorrect");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, Settings.Role)
            };

            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetConnectionString("AppId")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_configuration.GetConnectionString("ValidIssuer"),
                _configuration.GetConnectionString("ValidAudience"), claims, expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}