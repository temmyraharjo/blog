using System.Reflection;
using System.Text;
using FluentValidation;
using LearningCqrs.Contracts;
using LearningCqrs.Core.Swagger;
using LearningCqrs.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace LearningCqrs.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCore(this IServiceCollection serviceCollection)
    {
        AssemblyScanner.FindValidatorsInAssembly(typeof(Program).Assembly)
            .ForEach(item => serviceCollection.AddScoped(item.InterfaceType, item.ValidatorType));
        serviceCollection.AddMediatR(typeof(Program).GetTypeInfo().Assembly);   
        serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehavior<,>));
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    public static void AddCoreAuthentication(this IServiceCollection serviceCollection)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetConnectionString("AppId")));
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }

    public static void AddCoreSwaggerGeneration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(opt =>
        {
            var securityDefinition = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            };
            opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDefinition);
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityDefinition, Array.Empty<string>()}
            });
    
            opt.OperationFilter<AuthResponsesOperationFilter>();
            opt.SchemaFilter<SwaggerIgnoreFilter>();
            opt.OperationFilter<IgnorePropertyFilter>();
            opt.DocumentFilter<JsonPatchDocumentFilter>();
        });
    }
    
    public static void AddCoreDatabase(this IServiceCollection serviceCollection, string configurationName="BlogConnectionString")
    {
        serviceCollection.AddDbContext<BlogContext>(options =>
        {
            var connectionString = Configuration.GetConnectionString(configurationName);
            options.UseSqlServer(connectionString);
        });
    }

    public static void AddCoreAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(Contracts.IMapper), typeof(Features.Mapper));
    }
}