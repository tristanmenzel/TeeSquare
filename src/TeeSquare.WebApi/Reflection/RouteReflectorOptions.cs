using System;
using System.Reflection;
using TeeSquare.Reflection;
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
                FunctionWriterFactory = FunctionWriterFactory,
                ComplexTypeStrategy = ComplexTypeStrategy,
                WriteHeader = WriteHeader,
                MethodFlags = MethodFlags,
                ReflectMethods = ReflectMethods
            };
        }

        public RouteNamer Namer { get; set; } = new RouteNamer();

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public BindingFlags MethodFlags { get; set; } = BindingFlags.Instance
                                                        | BindingFlags.Public
                                                        | BindingFlags.DeclaredOnly;

        public Func<Type, bool> ReflectMethods = type => false;

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public string IndentChars { get; set; } = "  ";
        public bool WriteEnumAllValuesConst { get; set; }


        public IEnumWriterFactory EnumWriterFactory { get; set; } = new EnumWriterFactory();
        public IInterfaceWriterFactory InterfaceWriterFactory { get; set; } = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory { get; set; } = new ClassWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory { get; set; } = new FunctionWriterFactory();

        public WriteComplexType ComplexTypeStrategy { get; set; } =
            (writer, typeInfo) => writer.WriteInterface(typeInfo);


        public GetApiReturnType GetApiReturnTypeStrategy { get; set; } = RouteReflector.DefaultApiReturnTypeStrategy;
        public BuildRoute BuildRouteStrategy { get; set; } = RouteReflector.DefaultBuildRouteStrategy;
        public WriteHeader WriteHeader { get; set; } = writer =>
        {
            writer.WriteComment("Generated Code");
            writer.WriteLine();
        };

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } =
            WriterOptions.DefaultDiscriminator;

        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } =
            WriterOptions.DefaultDiscriminatorValueProvider;

    }

    public delegate Type GetApiReturnType(Type controller, MethodInfo action);

    public delegate string BuildRoute(Type controller, MethodInfo action);
}
