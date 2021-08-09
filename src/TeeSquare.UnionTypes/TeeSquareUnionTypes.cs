using System;
using System.Linq;
using System.Reflection;
using TeeSquare.Configuration;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;

namespace TeeSquare.UnionTypes
{
    public static class TeeSquareUnionTypes
    {
        public static void Configure(IReflectiveWriterOptions options)
        {
            options.GetTypeDependenciesStrategy = options.GetTypeDependenciesStrategy.ExtendStrategy(original =>
                (type, o) =>
                {
                    var uta = type.GetCustomAttribute<UnionTypeAttribute>();
                    return Enumerable.Union(original(type, o), uta?.UnionTypes ?? Array.Empty<Type>()).ToArray();
                });
            options.ComplexTypeStrategy = options.ComplexTypeStrategy.ExtendStrategy(original =>
                (writer, typeInfo, type) =>
                {
                    var uta = type.GetCustomAttribute<UnionTypeAttribute>();
                    if (uta == null)
                    {
                        original(writer, typeInfo, type);
                    }
                    else
                    {
                        var unionTypes = String.Join(" | ", uta.UnionTypes
                            .Select(t => options.TypeConverter.Convert(t).FullName));

                        writer.WriteLine($"export type {typeInfo.TypeReference.FullName} = {unionTypes};");
                    }
                });
            options.TypeConverter.Convert = options.TypeConverter.Convert.ExtendStrategy(original =>
                (type, parent, memberInfo) =>
                {
                    if (memberInfo is PropertyInfo pi)
                    {
                        var constAttr = pi.GetCustomAttribute<AsConstAttribute>();
                        if (constAttr != null)
                        {
                            string literalType;
                            var value = constAttr.ConstValue;
                            if (value is Boolean b)
                                literalType = b ? "true" : "false";
                            else if (value is String s)
                                literalType = $"'{s}'";
                            else if (value is Enum e)
                            {
                                var enumTypeName = options.TypeConverter.TypeName(e.GetType());
                                var enumMemberName = options.TypeConverter.EnumName(e.ToString());
                                literalType = $"{enumTypeName}.{enumMemberName}";
                            }
                            else
                                literalType = value.ToString() ?? string.Empty;

                            return new TypeReference(literalType) {ExistingType = true};
                        }
                    }

                    return original(type, parent, memberInfo);
                });
        }
    }
}
