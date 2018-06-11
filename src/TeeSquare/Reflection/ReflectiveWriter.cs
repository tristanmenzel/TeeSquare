using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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
            foreach (var type in types)
            {
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

                if (type.IsValueType && !type.IsEnum) continue;
                if (type == typeof(string)) continue;

                if (!_types.Contains(type))
                {
                    _types.Add(type);

                    if (type.IsEnum) continue;

                    var dependencies = type.GetProperties().Select(p => p.PropertyType).ToArray();
                    AddTypes(dependencies);
                }
            }
        }

        public void WriteTo(TypeScriptWriter writer)
        {
            foreach (var type in _types.OrderBy(t => t.FullName))
            {
                if (type.IsEnum)
                {
                    writer.WriteEnum(_namer.TypeName(type))
                        .WithValues(e =>
                        {
                            foreach (var field in Enum.GetNames(type).Zip(Enum.GetValues(type).Cast<int>(),
                                (name, value) => new {name, value}))
                            {
                                string description = null;
                                if (_options.WriteEnumDescriptions)
                                {
                                    description = type.GetMember(field.name)
                                        .SelectMany(m => m.GetCustomAttributes<DescriptionAttribute>())
                                        .Select(a => a.Description)
                                        .FirstOrDefault();
                                }

                                e.Value(_namer.EnumName(field.name), field.value, description);
                            }
                        })
                        .IncludeDescriptionGetter(_options.WriteEnumDescriptionGetters);
                    continue;
                }


                writer.WriteInterface(_namer.TypeName(type))
                    .With(i =>
                    {
                        foreach (var pi in type.GetProperties(_options.PropertyFlags))
                        {
                            if (pi.PropertyType.IsNullable(out var underlyingType))
                            {
                                i.Property(_namer.PropertyName(pi) + "?", _namer.TypeName(underlyingType));
                                continue;
                            }

                            if (pi.PropertyType.IsCollection(out var itemType))
                            {
                                i.Property(_namer.PropertyName(pi), _namer.TypeName(itemType) + "[]");
                                continue;
                            }

                            i.Property(_namer.PropertyName(pi), _namer.TypeName(pi.PropertyType));
                        }
                    });
            }

            writer.Flush();
        }
    }
}