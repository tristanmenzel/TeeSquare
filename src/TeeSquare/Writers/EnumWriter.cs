using System;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class EnumWriter : ICodePart
    {
        private readonly EnumInfo _enumInfo;
        private bool _includeDescriptionGetter;
        private bool _includeAllValuesConstant;
        private Action<IEnumInfo, ICodeWriter> _customCodeWriter;

        public EnumWriter(string name)
        {
            _enumInfo = new EnumInfo(name);
        }

        public EnumWriter With(Action<IEnumConfigurator> configure)
        {
            configure(_enumInfo);
            return this;
        }

        public EnumWriter IncludeDescriptionGetter(bool include)
        {
            _includeDescriptionGetter = include;
            return this;
        }

        public EnumWriter IncludeAllValuesConst(bool include)
        {
            _includeAllValuesConstant = include;
            return this;
        }

        public EnumWriter IncludeCustomCode(Action<IEnumInfo, ICodeWriter> customCodeWriter)
        {
            _customCodeWriter = customCodeWriter;
            return this;
        }

        private ICodePart BuildDescGetterFunc()
        {
            return new FunctionWriter($"Get{_enumInfo.Name}Description")
                .WithReturnType("string")
                .WithParams(p => p.Param("value", _enumInfo.Name))
                .AsConstArrows()
                .WithBody(body => { body.WriteLine($"return {_enumInfo.Name}Desc[value];"); });
        }

        void ICodePart.WriteTo(ICodeWriter writer)
        {
            writer.OpenBrace($"export enum {_enumInfo.Name}");
            writer.WriteDelimitedLines(_enumInfo.Values, v => $"{v.Name} = {v.FormattedValue}", ",");
            writer.CloseBrace();

            if (_includeAllValuesConstant)
            {
                writer.WriteLine($"export const All{_enumInfo.Name}: {_enumInfo.Name}[] = [");
                writer.Indent();
                writer.WriteDelimitedLines(_enumInfo.Values, v => $"{_enumInfo.Name}.{v.Name}", ",");
                writer.Deindent();
                writer.WriteLine("];");
            }

            if (_enumInfo.Values.Any(v => v.Description != null))
            {
                writer.OpenBrace($"export const {_enumInfo.Name}Desc: {{ [key: number]: string }} =");
                writer.WriteDelimitedLines(_enumInfo.Values, v => $"{v.FormattedValue}: `{v.Description ?? v.Name}`",
                    ",");
                writer.CloseBrace(true);
                if (_includeDescriptionGetter)
                {
                    BuildDescGetterFunc().WriteTo(writer);
                }
            }

            if (_customCodeWriter != null)
            {
                _customCodeWriter(_enumInfo, writer);
            }
        }
    }
}