using backend.Data;
using FluentValidation;
using System;

namespace backend.Features.User
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<Create.Command, Create.Command> UserNameUnique(
            this IRuleBuilder<Create.Command, Create.Command> ruleBuilder,
            BlogContext query) => UserNameUnique(ruleBuilder, query, m => m.UserName);

        private static IRuleBuilderOptions<T, T> UserNameUnique<T>(
            IRuleBuilder<T, T> ruleBuilder,
            BlogContext query,
            Func<T, string> getUserName)
        {
            return ruleBuilder.MustAsync(async (root, model, context) =>
            {
                var userName = getUserName(model);
                var isExists = await new UserNameUniqueValidator(query).GetResult(userName);

                return !isExists;
            }).WithMessage("Username is already exists!");
        }
    }
}