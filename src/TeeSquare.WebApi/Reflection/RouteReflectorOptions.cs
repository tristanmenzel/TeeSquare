using System;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    internal class RouteReflectorOptions : IRouteReflectorOptions
    {
        public string DefaultRoute { get; set; } = "{controller=Home}/{action=Index}/{id?}";

        public string RoutePrefix { get; set; } = "/";

        public FactoryNameStrategy FactoryNameStrategy { get; set; } = RouteReflector.DefaultFactoryNameStrategy;


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

        public Func<Type, (bool reflect, Assembly[] additionalAssemblies)> ReflectSubTypes { get; set; }
            = type => (false, Array.Empty<Assembly>());
        public Func<Type, MethodInfo, bool> ReflectMethod { get; set; } = (type, mi) => true;

        public GetTypeDependenciesStrategy GetTypeDependenciesStrategy { get; set; } =
            ReflectiveWriter.GetTypeDependencies;
        public string IndentCharacters { get; set; } = "  ";

        public IEnumWriterFactory EnumWriterFactory { get; set; } = new EnumWriterFactory();
        public IInterfaceWriterFactory InterfaceWriterFactory { get; set; } = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory { get; set; } = new ClassWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory { get; set; } = new FunctionWriterFactory();

        public ComplexTypeStrategy ComplexTypeStrategy { get; set; } =
            (writer, typeInfo, type) => writer.WriteInterface(typeInfo);


        public GetApiReturnType GetApiReturnTypeStrategy { get; set; } = RouteReflector.DefaultApiReturnTypeStrategy;
        public BuildRoute BuildRouteStrategy { get; set; } = RouteReflector.DefaultBuildRouteStrategy;
        public NameRoute NameRouteStrategy { get; set; } = RouteReflector.DefaultNameRouteStrategy;
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
        public EnumValueTypeStrategy EnumValueTypeStrategy { get; set; } = EnumValueTypeStrategies.AllNumber;

        public TypeReference EmptyRequestBodyType { get; set; } = new TypeReference("undefined");
        public string QueryVariableName { get; set; } = "query";
    }

    public delegate ParameterKind GetParameterKind(ParameterInfo parameterInfo, string route, HttpMethod method);

    public delegate Type GetApiReturnType(Type controller, MethodInfo action);

    public delegate string BuildRoute(Type controller, MethodInfo action, string defaultRoute);

    public delegate string NameRoute(Type controller, MethodInfo action, string route, HttpMethod method);

    public delegate string FactoryNameStrategy(Type controller, MethodInfo action, string route);

    public delegate (RequestFactory factory, HttpMethod method) GetHttpMethodAndRequestFactory(Type controller,
        MethodInfo action);
}
