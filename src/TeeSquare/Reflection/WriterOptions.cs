using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;
using PropertyInfo = System.Reflection.PropertyInfo;

namespace TeeSquare.Reflection
{
    public class WriterOptions
    {
        public Namer Namer { get; set; } = new Namer();

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                            | BindingFlags.Public
                                            | BindingFlags.Instance;

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public string IndentChars { get; set; } = "  ";
        public bool WriteEnumAllValuesConst { get; set; }

        public Action<IEnumInfo, ICodeWriter> CustomEnumWriter { get; set; }

        public Action<ITypeInfo, ICodeWriter> CustomInterfaceWriter { get; set; }

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } = DefaultDescriminator;
        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } = DefaultDescriminatorValueProvider;

        public static bool DefaultDescriminator(PropertyInfo property, Type parentType)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>().Any();
        }
        
        public static string DefaultDescriminatorValueProvider(PropertyInfo property, Type parentType)
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