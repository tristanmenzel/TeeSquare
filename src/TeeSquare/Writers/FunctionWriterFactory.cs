using System;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public interface IFunctionWriterFactory
    {
        CodeSnippetWriter Build(IMethodInfo method);
    }

    public class FunctionWriterFactory : IFunctionWriterFactory
    {
        private readonly bool _useArrows;

        public FunctionWriterFactory(bool useArrows = true)
        {
            _useArrows = useArrows;
        }

        public CodeSnippetWriter Build(IMethodInfo method)
        {
            return writer =>
            {
                if (_useArrows)
                {
                    writer.Write($"export const {method.Id.Name} = ");
                }
                else
                {
                    writer.Write($"export function {method.Id.Name}");
                }

                if (method.Id.GenericTypeParams.Any())
                {
                    writer.Write("<");
                    writer.Write(string.Join(", ", method.Id.GenericTypeParams));
                    writer.Write(">");
                }

                writer.Write("(");


                writer.WriteDelimited(method.Params,
                    (p, w) =>
                    {
                        w.Write($"{p.Name}: ");
                        w.WriteType(p.Type, p.GenericTypeParams);
                    }, ", ");

                writer.Write("): ");
                writer.WriteType(method.ReturnType.Type, method.ReturnType.GenericTypeParams);

                writer.Write(_useArrows ? " =>" : "");
                writer.OpenBrace();

                method.WriteBody(writer);

                writer.CloseBrace(_useArrows);
            };
        }
    }
}
