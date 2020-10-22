using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.Reflection
{
    public class ReflectiveWriter
    {
        private bool _importsWritten;
        private TypeConverter TypeConverter => _options.TypeConverter;
        private TypeConverter ImportTypeConverter => _options.ImportTypeConverter;
        private TypeCollection Types => _options.Types;
        private readonly IReflectiveWriterOptions _options;

        public ReflectiveWriter(IReflectiveWriterOptions options = null)
        {
            _options = options ?? new ReflectiveWriterOptions();
        }

        public void AddTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                if (type.IsExtendedPrimitive())
                {
                    if (!Types.Contains(type))
                        Types.AddLocal(type);
                    continue;
                }

                if (type.IsTask(out var resultType))
                {
                    AddTypes(resultType);
                    continue;
                }

                if (type.IsNullable(out var underlyingType))
                {
                    AddTypes(underlyingType);
                    continue;
                }

                if (type.IsCollection(out var itemType))
                {
                    AddTypes(itemType);
                    continue;
                }

                if (type.IsDictionary(out var keyValueTypes))
                {
                    AddTypes(keyValueTypes);
                    continue;
                }

                if (type.IsGenericType)
                {
                    AddTypes(type.GetGenericArguments());

                    var genericType = type.GetGenericTypeDefinition();

                    if (Types.Contains(genericType)) continue;

                    var dependencies = GetTypeDependencies(type);
                    if (dependencies.Any())
                    {
                        AddTypes(dependencies);
                    }

                    Types.AddLocal(genericType);
                    continue;
                }

                if (!Types.Contains(type))
                {
                    Types.AddLocal(type);

                    if (!type.IsEnum)
                    {
                        var dependencies = GetTypeDependencies(type);
                        if (dependencies.Any())
                        {
                            AddTypes(dependencies);
                        }
                    }
                }
            }
        }

        private Type[] GetTypeDependencies(Type type)
        {
            // Don't reflect dependencies on statically mapped types
            if (TypeConverter.HasStaticMapping(type))
                return new Type[0];
            var propertyDependencies = type
                .GetProperties(_options.PropertyFlags)
                .Select(p => p.PropertyType)
                .Distinct()
                .ToArray();
            var fieldDependencies = type
                .GetFields(_options.FieldFlags)
                .Select(f => f.FieldType)
                .Distinct()
                .ToArray();
            if (!_options.ReflectMethods(type))
                return propertyDependencies
                    .Union(fieldDependencies)
                    .ToArray();
            var methodDependencies = type
                .GetMethods(_options.MethodFlags)
                .Where(m => !m.IsSpecialName)
                .Where(m => _options.ReflectMethod(type, m))
                .SelectMany(m => m.GetParameters().Select(p => p.ParameterType).Union(new[] {m.ReturnType}))
                .Where(m => m != typeof(void))
                .ToArray();

            return propertyDependencies
                .Union(fieldDependencies)
                .Union(methodDependencies)
                .ToArray();
        }

        private string BuildImport(Type type)
        {
            if (ImportTypeConverter != null)
            {
                var typeName = ImportTypeConverter.Convert(type).TypeName;
                var importAs = TypeConverter.Convert(type).TypeName;

                if (typeName != importAs)
                    return $"{typeName} as {importAs}";
                return typeName;
            }

            return TypeConverter.Convert(type).TypeName;
        }

        private string BuildImport(string typeName, string importAs)
        {
            if (importAs != null)
            {
                return $"{typeName} as {importAs}";
            }

            return typeName;
        }

        public void WriteImports(TypeScriptWriter writer)
        {
            foreach (var importGroup in Types.ImportedLiterals.GroupBy(t => t.ImportFrom))
            {
                writer.WriteLine(
                    $"import {{ {(string.Join(", ", importGroup.Select(g => BuildImport(g.TypeName, g.ImportAs))))} }} from '{importGroup.Key}';");
            }

            foreach (var importGroup in Types.UsedImportedTypes.GroupBy(t => t.ImportFrom))
            {
                writer.WriteLine(
                    $"import {{ {(string.Join(", ", importGroup.Select(g => BuildImport(g.Type))))} }} from '{importGroup.Key}';");
            }

            _importsWritten = true;
        }

        public void WriteTo(TypeScriptWriter writer, bool includeHeader = true)
        {
            if (includeHeader)
                _options.WriteHeader(writer);

            if (!_importsWritten)
            {
                WriteImports(writer);
            }

            foreach (var type in Types.LocalTypes.OrderBy(t => t.Name))
            {
                var typeRef = TypeConverter.Convert(type);

                if (typeRef.ExistingType) continue;

                if (typeRef.Enum)
                {
                    writer.WriteEnum(typeRef.TypeName)
                        .Configure(e =>
                        {
                            foreach (var field in Enum.GetNames(type).Zip(Enum.GetValues(type).Cast<int>(),
                                (name, value) => new {name, value}))
                            {
                                var description = type.GetMember(field.name)
                                    .SelectMany(m => m.GetCustomAttributes<DescriptionAttribute>())
                                    .Select(a => a.Description)
                                    .FirstOrDefault();

                                e.AddValue(TypeConverter.EnumName(field.name), field.value, description);
                            }
                        });
                    continue;
                }

                _options.ComplexTypeStrategy(writer, new ComplexTypeInfo(typeRef.TypeName, typeRef.GenericTypeParams)
                    .Configure(i =>
                    {
                        foreach (var pi in type.GetProperties(_options.PropertyFlags))
                        {
                            i.AddProperty(TypeConverter.PropertyName(pi),
                                TypeConverter.Convert(pi.PropertyType, type, pi));
                        }

                        foreach (var fi in type.GetFields(_options.FieldFlags))
                        {
                            i.AddProperty(TypeConverter.FieldName(fi),
                                TypeConverter.Convert(fi.FieldType, type, fi));
                        }

                        if (_options.ReflectMethods(type))
                        {
                            foreach (var mi in type.GetMethods(_options.MethodFlags)
                                .Where(m => !m.IsSpecialName)
                                .Where(m => _options.ReflectMethod(type, m)))
                            {
                                i.AddMethod(mi.Name)
                                    .WithReturnType(TypeConverter.Convert(mi.ReturnType, type, mi))
                                    .WithParams(p =>
                                    {
                                        foreach (var pi in mi.GetParameters())
                                        {
                                            p.Param(pi.Name, TypeConverter.Convert(pi.ParameterType, type, mi));
                                        }
                                    });
                            }
                        }
                    })
                    .Done());
            }

            writer.Flush();
        }
    }
}
