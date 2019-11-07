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

        public string IndentChars { get; set; } = "  ";


        public IEnumWriterFactory EnumWriterFactory = new EnumWriterFactory();
        public IInterfaceWriterFactory InterfaceWriterFactory = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory = new ClassWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory = new FunctionWriterFactory();

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } = DefaultDiscriminator;
        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } = DefaultDiscriminatorValueProvider;

        public static bool DefaultDiscriminator(PropertyInfo property, Type parentType)
        {
            return property.GetCustomAttributes<TypeDiscriminatorAttribute>().Any();
        }

        public static string DefaultDiscriminatorValueProvider(PropertyInfo property, Type parentType)
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
