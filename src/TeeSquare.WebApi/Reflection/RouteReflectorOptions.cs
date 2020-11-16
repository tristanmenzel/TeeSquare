using System;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    public interface IRouteReflectorOptions : IReflectiveWriterOptions
    {
        /// <summary>
        /// The default route pattern to use in the absence of route attributes.
        ///
        /// eg. {controller=Home}/{action=Index}/{id?}
        /// </summary>
        string DefaultRoute { get; }

        /// <summary>
        /// A typeConverter instance used to determine the name of a dotnet Type in TypeScript
        /// </summary>
        RouteNamer RouteNamer { get; }

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

        /// <summary>
        /// The strategy to use to determine the kind of a parameter (ie. query string / route / body )
        /// </summary>
        GetParameterKind GetParameterKindStrategy { get; }

        /// <summary>
        /// The <see cref="TypeReference"/> to use to represent an empty body type (ie undefined, null, any)
        /// </summary>
        TypeReference EmptyRequestBodyType { get; }
    }


    public delegate ParameterKind GetParameterKind(ParameterInfo parameterInfo, string route, HttpMethod method);

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
            "PatchRequest",
            "OptionsRequest",
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
        public string DefaultRoute { get; set; } = "{controller=Home}/{action=Index}/{id?}";

        public RouteNamer RouteNamer { get; set; } = new RouteNamer();

        public TypeConverter TypeConverter { get; set; } =
            new TypeConverter(StaticConfig.Instance.DefaultStaticMappings);

        public TypeConverter ImportTypeConverter { get; set; } = null;

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public BindingFlags FieldFlags { get; set; } = BindingFlags.Public
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

        public GetParameterKind GetParameterKindStrategy { get; set; } = RouteReflector.GetParameterKind;

        public WriteHeader WriteHeader { get; set; } = writer =>
        {
            writer.WriteComment("Auto-generated Code - Do Not Edit");
            writer.WriteLine();
        };

        public TypeCollection Types { get; set; } = new TypeCollection();

        public TypeReference EmptyRequestBodyType { get; set; } = new TypeReference("undefined");
    }

    public delegate Type GetApiReturnType(Type controller, MethodInfo action);

    public delegate string BuildRoute(Type controller, MethodInfo action, string defaultRoute);

    public delegate (RequestFactory factory, HttpMethod method) GetHttpMethodAndRequestFactory(Type controller,
        MethodInfo action);
}
