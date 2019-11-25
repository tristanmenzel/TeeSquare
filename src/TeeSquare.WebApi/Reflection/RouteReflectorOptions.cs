using System;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    public interface IRouteReflectorOptions : IReflectiveWriterOptions
    {
        new RouteNamer Namer { get; }
        new RouteNamer ImportNamer { get; }
        GetApiReturnType GetApiReturnTypeStrategy { get; }
        BuildRoute BuildRouteStrategy { get; }
        RequestHelperTypeOptions RequestHelperTypeOption { get; }
    }

    public class RequestHelperTypeOptions
    {
        private RequestHelperTypeOptions(bool emitTypes, string importFrom)
        {
            ShouldEmitTypes = emitTypes;
            ImportFrom = importFrom;
        }

        public string[] Types => new[]
        {
            "GetRequest",
            "PutRequest",
            "PostRequest",
            "DeleteRequest",
            "toQuery"
        };

        public bool ShouldEmitTypes { get; }

        public string ImportFrom { get; }

        public static RequestHelperTypeOptions EmitTypes => new RequestHelperTypeOptions(true, null);

        public static RequestHelperTypeOptions ImportTypes(string importFrom) =>
            new RequestHelperTypeOptions(false, importFrom);
    }

    public class RouteReflectorOptions : IRouteReflectorOptions
    {
        public RouteNamer Namer { get; set; } = new RouteNamer();
        public RouteNamer ImportNamer { get; set; } = null;

        Namer IReflectiveWriterOptions.Namer => Namer;
        Namer IReflectiveWriterOptions.ImportNamer => ImportNamer;

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public BindingFlags MethodFlags { get; set; } = BindingFlags.Instance
                                                        | BindingFlags.Public
                                                        | BindingFlags.DeclaredOnly;

        public Func<Type, bool> ReflectMethods { get; set; } = type => false;
        public string IndentCharacters { get; set; } = "  ";

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public bool WriteEnumAllValuesConst { get; set; }


        public IEnumWriterFactory EnumWriterFactory { get; set; } = new EnumWriterFactory();
        public IInterfaceWriterFactory InterfaceWriterFactory { get; set; } = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory { get; set; } = new ClassWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory { get; set; } = new FunctionWriterFactory();

        public WriteComplexType ComplexTypeStrategy { get; set; } =
            (writer, typeInfo) => writer.WriteInterface(typeInfo);


        public GetApiReturnType GetApiReturnTypeStrategy { get; set; } = RouteReflector.DefaultApiReturnTypeStrategy;
        public BuildRoute BuildRouteStrategy { get; set; } = RouteReflector.DefaultBuildRouteStrategy;
        public RequestHelperTypeOptions RequestHelperTypeOption { get; set; } = RequestHelperTypeOptions.EmitTypes;

        public WriteHeader WriteHeader { get; set; } = writer =>
        {
            writer.WriteComment("Auto-generated Code - Do Not Edit");
            writer.WriteLine();
        };

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } =
            ReflectiveWriterOptions.DefaultDiscriminator;

        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } =
            ReflectiveWriterOptions.DefaultDiscriminatorValueProvider;

        public TypeCollection Types { get; set; } = new TypeCollection();
    }

    public delegate Type GetApiReturnType(Type controller, MethodInfo action);

    public delegate string BuildRoute(Type controller, MethodInfo action);
}
