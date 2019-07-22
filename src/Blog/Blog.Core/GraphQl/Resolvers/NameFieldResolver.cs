using System;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace Blog.Core.GraphQl.Resolvers
{
    public class NameFieldResolver : IFieldResolver
    {
        public object Resolve(ResolveFieldContext context)
        {
            var source = context.Source;
            if (source == null)
            {
                return null;
            }
            var name = char.ToUpperInvariant(context.FieldAst.Name[0]) + 
                context.FieldAst.Name.Substring(1);
            var value = GetPropValue(source, name);

            return value;
        }
        private static object GetPropValue(object src, string propName)
        {
            var value = src.GetType().GetProperty(propName).GetValue(src, null);

            if(propName == "RowVersion" && value != null)
            {
                return Convert.ToBase64String((byte[])value);
            }

            return value;
        }
    }
}
