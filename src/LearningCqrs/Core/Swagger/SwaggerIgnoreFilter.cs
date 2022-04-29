using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningCqrs.Core.Swagger;

public class SwaggerIgnoreFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
    {
        if (schema.Properties.Count == 0)
            return;

        const BindingFlags bindingFlags = BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance;
        var memberList = schemaFilterContext.Type // In v5.3.3+ use Type instead
            .GetFields(bindingFlags).Cast<MemberInfo>()
            .Concat(schemaFilterContext.Type // In v5.3.3+ use Type instead
                .GetProperties(bindingFlags));

        var excludedList = memberList.Where(m =>
                m.GetCustomAttribute<SwaggerIgnoreAttribute>()
                != null)
            .Select(m =>
                m.GetCustomAttribute<JsonPropertyAttribute>()
                    ?.PropertyName
                ?? CamelCase(m.Name));

        foreach (var excludedName in excludedList)
            if (schema.Properties.ContainsKey(excludedName))
                schema.Properties.Remove(excludedName);
    }

    private static string CamelCase(string s)
    {
        var x = s.Replace("_", "");
        if (x.Length == 0) return "null";
        x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])",
            m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
        return char.ToLower(x[0]) + x.Substring(1);
    }
}