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
                    writer.Write($"export const {method.Name} = ");
                }
                else
                {
                    writer.Write($"export function {method.Name}");
                }

                if (method.GenericTypeParams.Any())
                {
                    writer.Write("<");
                    writer.Write(string.Join(", ", method.GenericTypeParams));
                    writer.Write(">");
                }

                writer.Write("(");


                writer.WriteDelimited(method.Params,
                    (p, w) =>
                    {
                        w.Write($"{p.Name}{(p.Type.Optional ? "?": "")}: ");
                        w.WriteTypeRef(p.Type);
                    }, ", ");

                writer.Write("): ");
                writer.WriteTypeRef(method.ReturnType);

                writer.Write(_useArrows ? " =>" : "");
                writer.OpenBlock();

                method.WriteBody(writer);

                writer.CloseBlock(_useArrows ? "};" : "}");
            };
        }
    }
}
