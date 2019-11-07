using System;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public interface IEnumWriterFactory
    {
        CodeSnippetWriter Build(IEnumInfo enumInfo);
    }

    public class EnumWriterFactory : IEnumWriterFactory
    {
        private bool _includeDescriptionGetter;
        private bool _includeAllValuesConstant;
        private bool _includeDescriptions;

        public EnumWriterFactory()
        {
        }

        public EnumWriterFactory IncludeDescriptionGetter(bool include = true)
        {
            _includeDescriptionGetter = include;
            return this;
        }

        public EnumWriterFactory IncludeDescriptions(bool include = true)
        {
            _includeDescriptions = include;
            return this;
        }

        public EnumWriterFactory IncludeAllValuesConst(bool include = true)
        {
            _includeAllValuesConstant = include;
            return this;
        }

        private CodeSnippetWriter BuildDescGetterFunc(IEnumInfo enumInfo)
        {
            var methodInfo = new MethodInfo($"Get{enumInfo.Name}Description")
                .WithReturnType("string")
                .WithParams(p => p.Param("value", enumInfo.Name))
                .WithBody(body => { body.WriteLine($"return {enumInfo.Name}Desc[value];"); })
                .Done();
            return new FunctionWriterFactory().Build(methodInfo);
        }

        public CodeSnippetWriter Build(IEnumInfo enumInfo)
        {
            return writer =>
            {
                writer.OpenBrace($"export enum {enumInfo.Name}");
                writer.WriteDelimitedLines(enumInfo.Values, v => $"{v.Name} = {v.FormattedValue}", ",");
                writer.CloseBrace();

                if (_includeAllValuesConstant)
                {
                    writer.WriteLine($"export const All{enumInfo.Name}: {enumInfo.Name}[] = [");
                    writer.Indent();
                    writer.WriteDelimitedLines(enumInfo.Values, v => $"{enumInfo.Name}.{v.Name}", ",");
                    writer.Deindent();
                    writer.WriteLine("];");
                }

                if (enumInfo.Values.Any(v => v.Description != null) && _includeDescriptions)
                {
                    writer.OpenBrace($"export const {enumInfo.Name}Desc: {{ [key: number]: string }} =");
                    writer.WriteDelimitedLines(enumInfo.Values, v => $"{v.FormattedValue}: `{v.Description ?? v.Name}`",
                        ",");
                    writer.CloseBrace(true);
                    if (_includeDescriptionGetter)
                    {
                        BuildDescGetterFunc(enumInfo)(writer);
                    }
                }
            };
        }
    }
}
