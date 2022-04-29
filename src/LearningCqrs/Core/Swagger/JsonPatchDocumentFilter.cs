﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningCqrs.Core.Swagger;

// https://stackoverflow.com/questions/65599406/swagger-unexpected-api-patch-action-documentation-of-jsonpatchdocument-in-exampl
public class JsonPatchDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var schemas = swaggerDoc.Components.Schemas.ToList();
        foreach (var item in schemas)
        {
            if (!item.Key.Contains("JsonPatchDocument")) continue;
            if (item.Value.Properties.All(e => e.Key != "operations")) continue;
            swaggerDoc.Components.Schemas.Remove(item);
            swaggerDoc.Components.Schemas.Add(item.Key, item.Value.Properties["operations"]);
        }
    }
}