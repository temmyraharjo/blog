using Humanizer;

namespace Blog.Core.Core
{
    public static class JsonPatchExtensions
    {
        public static string GetPropertyName(this 
            Microsoft.AspNetCore.JsonPatch.Operations.Operation operation) =>
            operation.path.Substring(1).Pascalize();
    }
}
