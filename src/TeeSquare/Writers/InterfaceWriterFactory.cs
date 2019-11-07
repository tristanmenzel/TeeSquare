using System;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public interface IInterfaceWriterFactory
    {
        CodeSnippetWriter Build(IComplexTypeInfo typeInfo);
    }

    public class InterfaceWriterFactory : IInterfaceWriterFactory
    {

        public CodeSnippetWriter Build(IComplexTypeInfo typeInfo)
        {
            return writer =>
            {
                writer.Write($"export interface ");
                writer.WriteType(typeInfo.Name, typeInfo.GenericTypeParams);
                writer.OpenBrace();
                foreach (var prop in typeInfo.Properties)
                {
                    writer.Write($"{prop.Name}: ", true);
                    writer.WriteType(prop.Type, prop.GenericTypeParams);
                    writer.WriteLine(";", false);
                }

                foreach (var method in typeInfo.Methods)
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
            };
        }

    }
}
