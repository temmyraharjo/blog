using System;
using Blog.Core.Data;
using Blog.Core.GraphQl;
using Blog.Core.GraphQl.Models;
using Blog.Core.Models;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.Core.Tests.Graphql
{
    public class GraphQlQueryTests
    {
        private byte[] GenerateRowVersion()
        {
            return Convert.FromBase64String(Guid.NewGuid().ToString().Replace("-", ""));
        }

        [Fact]
        public void Testing_convert_byte_to_string()
        {
            var id = Guid.NewGuid().ToString().Replace("-", "");
            var byteResult = Convert.FromBase64String(id);
            var textResult = Convert.ToBase64String(byteResult);

            Assert.Equal(id, textResult);
        }

        [Fact]
        public async void Graphql_retrieveall_shouldvalid()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new BlogContext(options);

            var user = new User
            {
                FirstName = "User",
                LastName = "001",
                UserName = "user001",
                RowVersion = GenerateRowVersion()
            };

            context.Users.Add(user);
            context.SaveChanges();

            var tableNameLookup = new TableNameLookup();
            var databaseMetadata = new DatabaseMetadata(context, tableNameLookup);

            var schema = new Schema
            {
                Query = new GraphQlQuery(context, databaseMetadata, tableNameLookup)
            };
            schema.Initialize();

            var result = await new DocumentExecuter().ExecuteAsync(new ExecutionOptions
            {
                Schema = schema,
                Query = "{user_list(offset:0, first:5) {id, userName, firstName, lastName, rowVersion}}"
            });

            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async void Graphql_retrieve_shouldvalid()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new BlogContext(options);

            var user = new User
            {
                Id = 1,
                FirstName = "User",
                LastName = "001",
                UserName = "user001",
                RowVersion = GenerateRowVersion()
            };

            context.Users.Add(user);
            context.SaveChanges();

            var tableNameLookup = new TableNameLookup();
            var databaseMetadata = new DatabaseMetadata(context, tableNameLookup);

            var schema = new Schema
            {
                Query = new GraphQlQuery(context, databaseMetadata, tableNameLookup)
            };
            schema.Initialize();

            var id = "\"" + user.Id + "\"";
            var query = "{user(id:" + id + "){id, userName, firstName, lastName, rowVersion}}";
            var result = await new DocumentExecuter().ExecuteAsync(new ExecutionOptions
            {
                Schema = schema,
                Query = query
            });

            Assert.Null(result.Errors);
            Assert.NotNull(result.Data);
        }
    }
}
