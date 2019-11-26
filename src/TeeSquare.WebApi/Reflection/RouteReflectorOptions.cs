using System;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    public interface IRouteReflectorOptions : IReflectiveWriterOptions
    {
        /// <summary>
        /// A namer instance used to determine the name of a dotnet Type in TypeScript
        /// </summary>
        new RouteNamer Namer { get; }
        /// <summary>
        /// An optional alternative namer used to determine the name used for imports. If set,
        /// types will be imported with ImportName as Name
        /// </summary>
        new RouteNamer ImportNamer { get; }
        /// <summary>
        /// The strategy to use to determine the return type of an api method. Default is to use
        /// the return value.
        /// </summary>
        GetApiReturnType GetApiReturnTypeStrategy { get; }
        /// <summary>
        /// The strategy to use to determine the route of an api method. Default is to use
        /// dotnetcore Route attributes.
        /// </summary>
        BuildRoute BuildRouteStrategy { get; }
        /// <summary>
        /// The option for obtaining request helper types. Default is to emit them with each file.
        /// They can alternatively be imported.
        /// </summary>
        RequestHelperTypeOptions RequestHelperTypeOption { get; }
        /// <summary>
        /// The strategy to use to determine the http method of an api method. Should return a RequestFactory
        /// for that request type too. Default strategy looks for dotnetcore HttpGet/Put etc attributes
        /// </summary>
        GetHttpMethodAndRequestFactory GetHttpMethodAndRequestFactoryStrategy { get; }
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
        public Func<Type, MethodInfo, bool> ReflectMethod { get; set; } = (type, mi) => true;
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

        public GetHttpMethodAndRequestFactory GetHttpMethodAndRequestFactoryStrategy { get; set; } =
            RouteReflector.DefaultGetHttpMethodAndRequestFactory;

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

    public delegate (RequestFactory factory, HttpMethod method) GetHttpMethodAndRequestFactory(Type controller, MethodInfo action);
}
