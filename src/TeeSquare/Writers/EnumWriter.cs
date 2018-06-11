using System;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class EnumWriter : ICodePart
    {
        private readonly EnumTypeConfigurator _config;
        private bool _includeDescriptionGetter;
        private bool _includeAllValuesConstant;
        public string Name { get; }

        public EnumWriter(string name)
        {
            Name = name;
            _config = new EnumTypeConfigurator();
        }

        public EnumWriter WithValues(Action<IEnumTypeConfigurator> configure)
        {
            configure(_config);
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

        private ICodePart BuildDescGetterFunc()
        {
            return new FunctionWriter($"Get{Name}Description")
                .WithReturnType("string")
                .WithParams(p => p.Param("value", Name))
                .AsConstArrows()
                .WithBody(body => { body.WriteLine($"return {Name}Desc[value];"); });
        }

        void ICodePart.WriteTo(ICodeWriter writer)
        {
            writer.OpenBrace($"export enum {Name}");
            writer.WriteDelimitedLines(_config.Values, v => $"{v.Name} = {v.FormattedValue}", ",");
            writer.CloseBrace();

            if (_includeAllValuesConstant)
            {
                writer.WriteLine($"export const All{Name}: {Name}[] = [");
                writer.Indent();
                writer.WriteDelimitedLines(_config.Values, v => $"{Name}.{v.Name}", ",");
                writer.Deindent();
                writer.WriteLine("];");
            }

            if (_config.Values.Any(v => v.Description != null))
            {
                writer.OpenBrace($"export const {Name}Desc: {{ [key: number]: string }} =");
                writer.WriteDelimitedLines(_config.Values, v => $"{v.FormattedValue}: `{v.Description ?? v.Name}`",
                    ",");
                writer.CloseBrace(true);
                if (_includeDescriptionGetter)
                {
                    BuildDescGetterFunc().WriteTo(writer);
                }
            }
        }
    }
}