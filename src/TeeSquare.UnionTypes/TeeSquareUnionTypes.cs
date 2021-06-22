using System;
using System.Linq;
using System.Reflection;
using TeeSquare.Configuration;
using TeeSquare.Reflection;

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
        }
    }
}
