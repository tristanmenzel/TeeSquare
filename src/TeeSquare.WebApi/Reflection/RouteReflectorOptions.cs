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
                IndentChars = IndentChars,
                DiscriminatorPropertyPredicate = DiscriminatorPropertyPredicate,
                DiscriminatorPropertyValueProvider = DiscriminatorPropertyValueProvider,
                EnumWriterFactory = EnumWriterFactory,
                InterfaceWriterFactory = InterfaceWriterFactory,
                ClassWriterFactory = ClassWriterFactory,
                FunctionWriterFactory = FunctionWriterFactory
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



        public IEnumWriterFactory EnumWriterFactory = new EnumWriterFactory();
        public IInterfaceWriterFactory InterfaceWriterFactory = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory = new ClassWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory = new FunctionWriterFactory();

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } =
            WriterOptions.DefaultDiscriminator;

        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } =
            WriterOptions.DefaultDiscriminatorValueProvider;
    }
}
