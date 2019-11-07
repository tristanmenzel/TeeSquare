using System;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    public class RouteReflectorOptions
    {
        public WriterOptions BuildWriterOptions()
        {
            return new WriterOptions
            {
                Namer = Namer,
                PropertyFlags = PropertyFlags,
                WriteEnumDescriptions = WriteEnumDescriptions,
                WriteEnumDescriptionGetters = WriteEnumDescriptionGetters,
                IndentChars = IndentChars,
                WriteEnumAllValuesConst = WriteEnumAllValuesConst,
                CustomEnumWriter = CustomEnumWriter,
                CustomInterfaceWriter = CustomInterfaceWriter,
                DiscriminatorPropertyPredicate = DiscriminatorPropertyPredicate,
                DiscriminatorPropertyValueProvider = DiscriminatorPropertyValueProvider
            };
        }

        public RouteNamer Namer { get; set; } = new RouteNamer();

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public string IndentChars { get; set; } = "  ";
        public bool WriteEnumAllValuesConst { get; set; }

        public Action<IEnumInfo, ICodeWriter> CustomEnumWriter { get; set; }

        public Action<ITypeInfo, ICodeWriter> CustomInterfaceWriter { get; set; }

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } =
            WriterOptions.DefaultDescriminator;

        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } =
            WriterOptions.DefaultDescriminatorValueProvider;
    }
}