using System;
using System.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;
using MethodInfo = System.Reflection.MethodInfo;

namespace TeeSquare.Reflection
{
    public class ReflectiveWriterOptions : IReflectiveWriterOptions, ITypeScriptWriterOptions
    {
        public TypeConverter TypeConverter { get; set; } = new TypeConverter();
        public TypeConverter ImportTypeConverter { get; set; } = null;

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public BindingFlags FieldFlags { get; set; } = BindingFlags.Public
                                                       | BindingFlags.Instance
                                                       | BindingFlags.Static;

        public BindingFlags MethodFlags { get; set; } = BindingFlags.Instance
                                                        | BindingFlags.Public
                                                        | BindingFlags.DeclaredOnly;

        public Func<Type, bool> ReflectMethods { get; set; } = type => false;

        public Func<Type, MethodInfo, bool> ReflectMethod { get; set; } = (type, mi) => true;

        public GetTypeDependenciesStrategy GetTypeDependenciesStrategy { get; set; } = ReflectiveWriter.GetTypeDependencies;

        public string IndentCharacters { get; set; } = "  ";

        public EnumValueTypeStrategy EnumValueTypeStrategy { get; set; } = EnumValueTypeStrategies.AllNumber;

        public IEnumWriterFactory EnumWriterFactory { get; set; } = new EnumWriterFactory();
        public IInterfaceWriterFactory InterfaceWriterFactory { get; set; } = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory { get; set; } = new ClassWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory { get; set; } = new FunctionWriterFactory();

        public ComplexTypeStrategy ComplexTypeStrategy { get; set; } =
            (writer, typeInfo, type) => writer.WriteInterface(typeInfo);

        public WriteHeader WriteHeader { get; set; } = writer =>
        {
            writer.WriteComment("Auto-generated Code - Do Not Edit");
            writer.WriteLine();
        };

        public TypeCollection Types { get; set; } = new TypeCollection();
    }

    public delegate void ComplexTypeStrategy(TypeScriptWriter writer, IComplexTypeInfo complexType, Type originalType);

    public delegate EnumValueType EnumValueTypeStrategy(Type type);

    public delegate Type[] GetTypeDependenciesStrategy(Type type, IReflectiveWriterOptions options);

    public delegate void WriteHeader(TypeScriptWriter writer);
}
