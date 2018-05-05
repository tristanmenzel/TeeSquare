using System;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class InterfaceWriter : ICodePart
    {
        private readonly string _name;
        private readonly string[] _genericTypeParams;
        private readonly TypeConfigurer _config;

        public InterfaceWriter(string name, string[] genericTypeParams)
        {
            _name = name;
            _genericTypeParams = genericTypeParams;
            _config = new TypeConfigurer();
        }

        public void With(Action<ITypeConfigurer> configure)
        {
            configure(_config);
        }


        public void WriteTo(ICodeWriter writer)
        {
            writer.Write($"export interface ");
            writer.WriteType(_name, _genericTypeParams);
            writer.OpenBrace();
            foreach (var prop in _config.Properties)
            {
                writer.Write($"{prop.Name}: ", true);
                writer.WriteType(prop.Type, prop.GenericTypeParams);
                writer.WriteLine(";", false);
            }

            foreach (var method in _config.Methods)
            {
                writer.Write(method.IsStatic ? "static " : string.Empty, true);
                writer.Write($"{method.Id.Name}( ");
                bool first = true;
                foreach (var param in method.Params)
                {
                    if (!first)
                    {
                        writer.Write(", ");
                    }

                    writer.Write($"{param.Name}: ");
                    writer.WriteType(param.Type, param.GenericTypeParams);
                    first = false;
                }

                writer.Write("): ");
                writer.WriteType(method.Id.Type, method.Id.GenericTypeParams);
                writer.WriteLine(";", false);
            }

            writer.CloseBrace();
        }
    }
}