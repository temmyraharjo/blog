﻿using System.Reflection;
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
        serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidateLookupPipelineBehavior<,>));
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    public static void AddCoreAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetConnectionString("AppId")));
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
                { securityDefinition, Array.Empty<string>() }
            });

            opt.OperationFilter<AuthResponsesOperationFilter>();
            opt.OperationFilter<IgnorePropertyFilter>();
            opt.SchemaFilter<SwaggerIgnoreFilter>();
            opt.DocumentFilter<JsonPatchDocumentFilter>();
        });
        serviceCollection.AddSwaggerGenNewtonsoftSupport();
    }

    public static void AddCoreDatabase(this IServiceCollection serviceCollection, IConfiguration configuration,
        string configurationName = "BlogConnectionString")
    {
        serviceCollection.AddDbContext<BlogContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(configurationName);
            options.UseSqlServer(connectionString);
        });
    }

    public static void AddCoreAutoMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(IMapper), typeof(Features.Mapper));
    }

    public static void AddCoreLogging(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var enableLogging = bool.Parse(configuration.GetConnectionString("Logging"));
        if (enableLogging)
        {
            serviceCollection.AddApplicationInsightsTelemetry();
            serviceCollection.AddLogging(config =>
            {
                config.AddApplicationInsights(configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            });
        }
        else
        {
            serviceCollection.AddLogging(config => config.AddConsole());
        }
    }
}