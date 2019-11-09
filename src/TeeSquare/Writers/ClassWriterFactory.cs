using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public interface IClassWriterFactory
    {
        CodeSnippetWriter Build(IComplexTypeInfo typeInfo);
    }

    public class ClassWriterFactory : IClassWriterFactory
    {
        public CodeSnippetWriter Build(IComplexTypeInfo typeInfo)
        {
            return writer =>
            {
                writer.Write($"export {(typeInfo.IsAbstract ? "abstract " : "")}class ");
                writer.WriteTypeRef(typeInfo.TypeReference);
                writer.OpenBlock();
                foreach (var prop in typeInfo.Properties)
                {
                    writer.Write($"{prop.Name}{(prop.Type.Optional ? "?":"")}: ", true);
                    writer.WriteTypeRef(prop.Type);
                    writer.WriteLine(";", false);
                }

                foreach (var method in typeInfo.Methods)
                {
                    writer.Write(method.IsStatic ? "static " : string.Empty, true);
                    writer.Write($"{method.Name}(");

                    writer.WriteDelimited(method.Params,
                        (p, w) =>
                        {
                            w.Write($"{p.Name}{(p.Type.Optional ? "?": "")}: ");
                            w.WriteTypeRef(p.Type);
                        }, ", ");

                    writer.Write("): ");
                    writer.WriteTypeRef(method.ReturnType);
                    writer.OpenBlock();

                    method.WriteBody(writer);

                    writer.CloseBlock();
                }

                writer.CloseBlock();
            };
        }
    }
}
