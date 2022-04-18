using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LearningCqrs.Core.Swagger;

// https://stackoverflow.com/questions/67570419/ignore-fromform-data-from-rendering-on-swagger
public class IgnorePropertyFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription == null || operation.Parameters == null)
            return;

        if (!context.ApiDescription.ParameterDescriptions.Any())
            return;


        var excludedProperties = context.ApiDescription.ParameterDescriptions.Where(p =>
            p.Source.Equals(BindingSource.Form));

        var apiParameterDescriptions = excludedProperties as ApiParameterDescription[] ?? excludedProperties.ToArray();
        if (apiParameterDescriptions.Any())
        {

            foreach (var apiParameterDescription in apiParameterDescriptions)
            {
                foreach (var customAttribute in apiParameterDescription.CustomAttributes())
                {
                    if (customAttribute.GetType() != typeof(SwaggerIgnoreAttribute)) continue;
                    for (var i = 0; i < operation.RequestBody.Content.Values.Count; i++)
                    {
                        for (var j = 0; j < operation.RequestBody.Content.Values.ElementAt(i).Encoding.Count; j++)
                        {
                            if (operation.RequestBody.Content.Values.ElementAt(i).Encoding.ElementAt(j).Key !=
                                apiParameterDescription.Name) continue;
                            operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                .Remove(operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                    .ElementAt(j));
                            operation.RequestBody.Content.Values.ElementAt(i).Schema.Properties.Remove(apiParameterDescription.Name);
                        }
                    }
                }
            }

        }
    }
}