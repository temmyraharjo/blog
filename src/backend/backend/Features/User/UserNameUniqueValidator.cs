﻿using System.Threading.Tasks;
using backend.Core;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Features.User
{
    public class UserNameUniqueValidator : BaseValidator<string>
    {
        public UserNameUniqueValidator(BlogContext context) : base(context)
        {
        }

        public override async Task<bool> GetResult(string input)
        {
            var result = await Context.Users.AnyAsync(e => e.UserName == input);
            return result;
        }
    }
}