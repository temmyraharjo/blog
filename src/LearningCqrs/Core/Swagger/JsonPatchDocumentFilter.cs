﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningCqrs.Core.Swagger;

// https://stackoverflow.com/questions/65599406/swagger-unexpected-api-patch-action-documentation-of-jsonpatchdocument-in-exampl
public class JsonPatchDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var schemas = swaggerDoc.Components.Schemas.ToList();
        for (var index = 0; index < schemas.Count; index++)
        {
            var item = schemas[index];
            if (item.Key.StartsWith("Operation") || item.Key.StartsWith("JsonPatchDocument") || item.Key == "IContractResolver")
                swaggerDoc.Components.Schemas.Remove(item.Key);
        }

        swaggerDoc.Components.Schemas.Add("Operation", new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                { "op", new OpenApiSchema { Type = "string" } },
                { "value", new OpenApiSchema { Type = "string" } },
                { "path", new OpenApiSchema { Type = "string" } }
            }
        });

        swaggerDoc.Components.Schemas.Add("JsonPatchDocument", new OpenApiSchema
        {
            Type = "array",
            Items = new OpenApiSchema
            {
                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "Operation" }
            },
            Description = "Array of operations to perform"
        });

        foreach (var path in swaggerDoc.Paths.SelectMany(p => p.Value.Operations)
                     .Where(p => p.Key == OperationType.Patch))
        {
            foreach (var item in path.Value.RequestBody.Content.Where(c => c.Key != "application/json-patch+json"))
                path.Value.RequestBody.Content.Remove(item.Key);

            var response = path.Value.RequestBody
                .Content.FirstOrDefault(c => c.Key == "application/json-patch+json");
            if (!string.IsNullOrEmpty( response.Key))
            {
                response.Value.Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "JsonPatchDocument" }
                };
                continue;
            }

            path.Value.RequestBody.Content.Add("application/json-patch+json", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "JsonPatchDocument" }
                }
            });
        }
    }
}