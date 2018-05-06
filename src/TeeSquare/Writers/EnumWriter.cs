using System;
using System.Linq;

namespace TeeSquare.Writers
{
    public class EnumWriter : ICodePart
    {
        private readonly EnumTypeConfigurator _config;
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

        public void WriteTo(ICodeWriter writer)
        {
            writer.OpenBrace($"export enum {Name}");
            writer.WriteDelimitedLines(_config.Values, v => $"{v.Name} = {v.FormattedValue}", ",");
            writer.CloseBrace();

            if (_config.Values.Any(v => v.Description != null))
            {
                writer.OpenBrace($"export const {Name}Desc =");
                writer.WriteDelimitedLines(_config.Values, v => $"{v.FormattedValue}: `{v.Description}`", ",");
                writer.CloseBrace();
            }
        }
    }
}