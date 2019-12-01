using System;
using System.Linq;
using System.Reflection;
using TeeSquare.TypeMetadata;
using PropertyInfo = System.Reflection.PropertyInfo;

namespace TeeSquare.Reflection
{
    public static class DiscriminatedUnionsHelper
    {
        private static bool IsDiscriminator(PropertyInfo property, Type parentType)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>().Any();
        }

        private static string GetDiscriminatorValue(PropertyInfo property, Type parentType)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>()
                       .Select(a => a.Value)
                       .FirstOrDefault() ?? parentType.Name;
        }

        public static bool DiscriminatorPropertyOverride(Type parentType, PropertyInfo property, Namer namer,
            out (string propertyName, TypeReference type) propertyDescriptor)
        {
            if (IsDiscriminator(property, parentType))
            {
                var value = GetDiscriminatorValue(property, parentType);
                propertyDescriptor = (namer.PropertyName(property), new TypeReference($"'{value}'") {ExistingType = true});
                return true;
            }

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
