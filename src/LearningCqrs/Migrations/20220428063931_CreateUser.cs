using LearningCqrs.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

#nullable disable

namespace LearningCqrs.Migrations
{
    public partial class CreateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                Username = "admin",
                FirstName = "Temmy Wahyu",
                LastName = "Raharjo"
            };
            var password = passwordHasher.HashPassword(user, "Password");
            user.Password = password;
            migrationBuilder.InsertData("Users", new[] { "Id", "Username", "Password", "FirstName", "LastName" },
                new[] { Guid.NewGuid().ToString(), user.Username, user.Password, user.FirstName, user.LastName });
        }
    }
}
