using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

namespace TeeSquare.Reflection
{
    public class WriterOptions
    {
        public Namer Namer { get; set; } = Namer.Default;

        public BindingFlags PropertyFlags = BindingFlags.GetProperty
                                            | BindingFlags.Public
                                            | BindingFlags.Instance;

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public string IndentChars { get; set; } = "  ";
        public bool WriteEnumAllValuesConst { get; set; }

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } = DefaultDescriminator;
        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } = DefaultDescriminatorValueProvider;

        private static bool DefaultDescriminator(PropertyInfo property, Type parentType)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>().Any();
        }
        private static string DefaultDescriminatorValueProvider(PropertyInfo property, Type parentType)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>()
                       .Select(a => a.Value)
                       .FirstOrDefault() ?? parentType.Name;
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

    public delegate bool DiscriminatorPropertyPredicate(PropertyInfo property, Type parentType);
    public delegate string DiscriminatorPropertyValueProvider(PropertyInfo property, Type parentType);
}