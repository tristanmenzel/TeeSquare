using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace TeeSquare.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool IsNullableReference(this PropertyInfo property)
        {
            if (property.PropertyType.IsValueType)
                return false;

            var nullable = property.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (nullable != null && nullable.ConstructorArguments.Count == 1)
            {
                var attributeArgument = nullable.ConstructorArguments[0];
                if (attributeArgument.ArgumentType == typeof(byte[]))
                {
                    var args = (ReadOnlyCollection<CustomAttributeTypedArgument>?)attributeArgument.Value;
                    if (args?.Count > 0 && args[0].ArgumentType == typeof(byte))
                    {
                        return (byte?)args[0].Value == 2;
                    }
                }
                else if (attributeArgument.ArgumentType == typeof(byte))
                {
                    return (byte?)attributeArgument.Value == 2;
                }
            }

            var context = property.DeclaringType?.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
            if (context != null &&
                context.ConstructorArguments.Count == 1 &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte?)context.ConstructorArguments[0].Value == 2;
            }

            // Couldn't find a suitable attribute
            return false;
        }
    }
}
