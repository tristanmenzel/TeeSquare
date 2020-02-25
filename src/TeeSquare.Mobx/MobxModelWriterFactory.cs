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
        internal static string MakeOptional(this string input, bool isOptional, string optionalType = "types.maybe({0})")
        {
            return isOptional ?  string.Format(optionalType, input) : input;
        }

        internal static string MakeCollection(this string input, bool isCollection, string collectionType = "types.array({0})")
        {
            return isCollection ?  string.Format(collectionType, input): input;
        }

        internal static string MakeEnum(this string input, bool isEnum, string enumType = "types.frozen<{0}>()")
        {
            return isEnum ? string.Format(enumType, input) : input;
        }
    }

    public class MobxModelWriterFactory : IMobxModelWriterFactory
    {
        private readonly IMobxOptions _options;

        public MobxModelWriterFactory(IMobxOptions options)
        {
            _options = options;
        }

        protected virtual string WrapType(ITypeReference type)
        {
            return type.TypeName
                .MakeEnum(type.Enum, _options.EnumType)
                .MakeCollection(type.Array, _options.CollectionType)
                .MakeOptional(type.Optional, _options.OptionalType);
        }

        public CodeSnippetWriter BuildModel(IComplexTypeInfo typeInfo)
        {
            return writer =>
            {
                writer.OpenBlock(
                    $"export const {typeInfo.Name}Props =");
                foreach (var prop in typeInfo.Properties)
                {
                    writer.Write($"{prop.Name}: ", true);
                    writer.Write(WrapType(prop.Type));
                    writer.WriteLine(",", false);
                }
                writer.CloseBlock();


                writer.OpenBlock(
                    $"export const {typeInfo.Name} = types.model('{typeInfo.Name}',");
                writer.WriteLine($"...{typeInfo.Name}Props");
                writer.CloseBlock("});");
                writer.WriteLine("");
                if (_options.EmitInstanceType)
                {
                    writer.WriteLine(
                        $"export type {_options.InstanceTypeName(typeInfo.Name)} = Instance<typeof {typeInfo.Name}>;");
                    writer.WriteLine("");
                }
            };
        }
    }
}
