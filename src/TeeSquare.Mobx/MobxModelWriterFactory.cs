using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.Mobx
{
    public interface IMobxModelWriterFactory
    {
        CodeSnippetWriter BuildModel(IComplexTypeInfo typeInfo);
    }

    internal static class MobxTypeExtensions
    {
        internal static string MakeOptional(this string input, bool isOptional, string optionalType = "types.maybe")
        {
            return isOptional ? $"{optionalType}({input})" : input;
        }

        internal static string MakeCollection(this string input, bool isCollection)
        {
            return isCollection ? $"types.array({input})" : input;
        }
    }

    public class MobxModelWriterFactory : IMobxModelWriterFactory
    {
        private readonly IMobxOptions _options;

        public MobxModelWriterFactory(IMobxOptions options)
        {
            _options = options;
        }

        protected virtual string GetMobxType(ITypeReference type)
        {
            return GetMobxBaseType(type)
                .MakeCollection(type.Array)
                .MakeOptional(type.Optional, _options.OptionalType);
        }

        protected virtual string GetMobxBaseType(ITypeReference type)
        {
            if (type.Enum)
            {
                return $"types.frozen<{type.TypeName}>()";
            }

            return type.TypeName switch
            {
                "number" => type.Format switch
                {
                    TsTypeFormat.Integer => _options.IntegerType,
                    TsTypeFormat.Identity => _options.NumericIdentityType,
                    _ => _options.DecimalType
                },
                "string" => type.Format switch
                {
                    TsTypeFormat.DateTime => _options.DateType,
                    TsTypeFormat.Identity => _options.StringIdentityType,
                    TsTypeFormat.Guid => _options.StringType,
                    _ => _options.StringType
                } ,
                "boolean" => _options.BooleanType,
                _ => type.ExistingType ? type.TypeName : $"{type.TypeName}Model"
            };
        }

        public CodeSnippetWriter BuildModel(IComplexTypeInfo typeInfo)
        {
            return writer =>
            {
                writer.OpenBlock($"export const {typeInfo.Name}Model = types.model('{typeInfo.Name}Model', ");
                foreach (var prop in typeInfo.Properties)
                {
                    writer.Write($"{prop.Name}: ", true);
                    writer.Write(GetMobxType(prop.Type));
                    writer.WriteLine(",", false);
                }

                writer.CloseBlock("});");
                writer.WriteLine("");
                writer.WriteLine($"export type {typeInfo.Name} = Instance<typeof {typeInfo.Name}Model>;");
            };
        }
    }
}
