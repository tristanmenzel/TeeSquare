using System;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class InterfaceWriter : ICodePart
    {
        private readonly TypeInfo _typeInfo;
        private Action<ITypeInfo, ICodeWriter> _customCodeWriter;

        public InterfaceWriter(string name, string[] genericTypeParams)
        {
            _typeInfo = new TypeInfo(name, genericTypeParams);
        }

        public InterfaceWriter With(Action<ITypeConfigurer> configure)
        {
            configure(_typeInfo);
            return this;
        }


        void ICodePart.WriteTo(ICodeWriter writer)
        {
            writer.Write($"export interface ");
            writer.WriteType(_typeInfo.Name, _typeInfo.GenericTypeParams);
            writer.OpenBrace();
            foreach (var prop in _typeInfo.Properties)
            {
                writer.Write($"{prop.Name}: ", true);
                writer.WriteType(prop.Type, prop.GenericTypeParams);
                writer.WriteLine(";", false);
            }

            foreach (var method in _typeInfo.Methods)
            {
                writer.Write(method.IsStatic ? "static " : string.Empty, true);
                writer.Write($"{method.Id.Name}(");
                writer.WriteDelimited(method.Params,
                    (p, w) =>
                    {
                        w.Write($"{p.Name}: ");
                        w.WriteType(p.Type, p.GenericTypeParams);
                    }, ", ");

                writer.Write("): ");
                writer.WriteType(method.ReturnType.Type, method.ReturnType.GenericTypeParams);
                writer.WriteLine(";", false);
            }

            writer.CloseBrace();
            if (_customCodeWriter != null)
            {
                _customCodeWriter(_typeInfo, writer);
            }
        }

        public InterfaceWriter IncludeCustomCode(Action<ITypeInfo, ICodeWriter> customCodeWriter)
        {
            _customCodeWriter = customCodeWriter;
            return this;
        }
    }
}