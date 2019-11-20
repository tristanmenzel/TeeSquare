using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.Reflection
{
    public class ReflectiveWriter
    {
        private readonly Namer _namer;
        private readonly HashSet<Type> _types = new HashSet<Type>();
        private readonly WriterOptions _options;

        public ReflectiveWriter(WriterOptions options = null)
        {
            _options = options ?? new WriterOptions();
            _namer = _options.Namer;
        }

        public void AddTypes(params Type[] types)
        {
            foreach (var type in types.OrderBy(t => t.Name))
            {
                if (type.IsExtendedPrimitive())
                {
                    if (!_types.Contains(type))
                        _types.Add(type);
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

                    if (_types.Contains(genericType)) continue;

                    var dependencies = GetTypeDependencies(type);
                    if (dependencies.Any())
                    {
                        AddTypes(dependencies);
                    }

                    _types.Add(genericType);
                    continue;
                }

                if (!_types.Contains(type))
                {
                    if (!type.IsEnum)
                    {
                        var dependencies = GetTypeDependencies(type);
                        if (dependencies.Any())
                        {
                            AddTypes(dependencies);
                        }
                    }

                    _types.Add(type);
                }
            }
        }

        private Type[] GetTypeDependencies(Type type)
        {
            var propertyDependencies = type
                .GetProperties(_options.PropertyFlags)
                .Select(p => p.PropertyType)
                .ToArray();
            if (!_options.ReflectMethods(type)) return propertyDependencies;
            var methodDependencies = type
                .GetMethods(_options.MethodFlags)
                .Where(m => !m.IsSpecialName)
                .SelectMany(m => m.GetParameters().Select(p => p.ParameterType).Union(new[] {m.ReturnType}))
                .Where(m => m != typeof(void))
                .ToArray();

            return propertyDependencies.Union(methodDependencies).ToArray();
        }

        public void WriteTo(TypeScriptWriter writer, bool includeHeader = true)
        {
            if (includeHeader)
                _options.WriteHeader(writer);

            foreach (var type in _types)
            {
                var typeRef = _namer.Type(type);

                if (typeRef.ExistingType) continue;

                if (typeRef.Enum)
                {
                    writer.WriteEnum(typeRef.TypeName)
                        .Configure(e =>
                        {
                            foreach (var field in Enum.GetNames(type).Zip(Enum.GetValues(type).Cast<int>(),
                                (name, value) => new {name, value}))
                            {
                                string description = null;
                                description = type.GetMember(field.name)
                                    .SelectMany(m => m.GetCustomAttributes<DescriptionAttribute>())
                                    .Select(a => a.Description)
                                    .FirstOrDefault();

                                e.AddValue(_namer.EnumName(field.name), field.value, description);
                            }
                        });
                    continue;
                }

                _options.ComplexTypeStrategy(writer, new ComplexTypeInfo(typeRef.TypeName, typeRef.GenericTypeParams)
                    .Configure(i =>
                    {
                        foreach (var pi in type.GetProperties(_options.PropertyFlags))
                        {
                            if (_options.DiscriminatorPropertyPredicate(pi, type))
                            {
                                var value = _options.DiscriminatorPropertyValueProvider(pi, type);
                                i.AddProperty(_namer.PropertyName(pi), new TypeReference($"'{value}'"));
                                continue;
                            }

                            i.AddProperty(_namer.PropertyName(pi), _namer.Type(pi.PropertyType));
                        }

                        if (_options.ReflectMethods(type))
                        {
                            foreach (var mi in type.GetMethods(_options.MethodFlags)
                                .Where(m => !m.IsSpecialName))
                            {
                                i.AddMethod(mi.Name)
                                    .WithReturnType(_namer.Type(mi.ReturnType))
                                    .WithParams(p =>
                                    {
                                        foreach (var pi in mi.GetParameters())
                                        {
                                            p.Param(pi.Name, _namer.Type(pi.ParameterType));
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
