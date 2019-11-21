using System;
using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using TeeSquare.Reflection;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    public interface IRouteReflectorOptions : IReflectiveWriterOptions
    {
        RouteNamer Namer { get; set; }
        bool EmitRequestTypesAndHelpers { get; set; }
        GetApiReturnType GetApiReturnTypeStrategy { get; set; }
        BuildRoute BuildRouteStrategy { get; set; }
        (string[] types, string path)[] Imports { get; set; }
    }

    public class RouteReflectorOptions: IRouteReflectorOptions
    {


        public RouteNamer Namer { get; set; } = new RouteNamer();

        Namer IReflectiveWriterOptions.Namer => Namer;

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public BindingFlags MethodFlags { get; set; } = BindingFlags.Instance
                                                        | BindingFlags.Public
                                                        | BindingFlags.DeclaredOnly;

        public Func<Type, bool> ReflectMethods { get; set; }= type => false;
        public string IndentCharacters { get; set; } = "  ";

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public bool WriteEnumAllValuesConst { get; set; }
        public bool EmitRequestTypesAndHelpers { get; set; } = true;


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

        public (string[] types, string path)[] Imports { get; set; }

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } =
            ReflectiveWriterOptions.DefaultDiscriminator;

        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } =
            ReflectiveWriterOptions.DefaultDiscriminatorValueProvider;

    }

    public delegate Type GetApiReturnType(Type controller, MethodInfo action);

    public delegate string BuildRoute(Type controller, MethodInfo action);
}
