using System;
using System.Linq;
using System.Reflection;
using TeeSquare.TypeMetadata;
using PropertyInfo = System.Reflection.PropertyInfo;

namespace TeeSquare.Reflection
{
    public static class DiscriminatedUnionsHelper
    {
        public static bool IsDiscriminator(Type parentType, PropertyInfo property)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>().Any();
        }

        public static string GetDiscriminatorValue(Type parentType, PropertyInfo property)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>()
                       .Select(a => a.Value)
                       .FirstOrDefault() ?? parentType.Name;
        }

        public static bool DiscriminatorPropertyOverride(Type parentType, PropertyInfo property, TypeConverter typeConverter,
            out (string propertyName, TypeReference type) propertyDescriptor)
        {


            propertyDescriptor = default;
            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TypeDiscriminatorAttribute : Attribute
    {
        public string Value { get; }

        public TypeDiscriminatorAttribute(string value = null)
        {
            Value = value;
        }
    }
}
