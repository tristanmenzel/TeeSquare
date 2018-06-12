using System;
using System.ComponentModel;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class ClassWriter : ICodePart
    {
        private readonly TypeInfo _typeInfo;

        public ClassWriter(string name, string[] genericTypeParams)
        {
            _typeInfo = new TypeInfo(name, genericTypeParams);
        }

        public ClassWriter Abstract()
        {
            _typeInfo.IsAbstract = true;
            return this;
        }

        public void With(Action<ITypeConfigurer> configure)
        {
            configure(_typeInfo);
        }


        void ICodePart.WriteTo(ICodeWriter writer)
        {
            writer.Write($"export {(_typeInfo.IsAbstract?"abstract ": "")}class ");
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
                writer.OpenBrace();

                method.WriteBody(writer);

                writer.CloseBrace();
            }

            writer.CloseBrace();
        }
    }
}