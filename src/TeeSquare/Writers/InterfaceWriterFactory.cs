using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public interface IInterfaceWriterFactory
    {
        CodeSnippetWriter Build(IComplexTypeInfo typeInfo);
    }

    public enum OptionalFieldRendering
    {
        WithQuestionMark,
        WithUndefinedUnion,
        WithNullUnion
    }

    public class InterfaceWriterFactory : IInterfaceWriterFactory
    {
        private readonly OptionalFieldRendering _optionalFieldRendering;

        public InterfaceWriterFactory(OptionalFieldRendering? optionalFieldRendering = null)
        {
            _optionalFieldRendering = optionalFieldRendering ?? OptionalFieldRendering.WithQuestionMark;
        }

        private bool UseQuestionMark => _optionalFieldRendering == OptionalFieldRendering.WithQuestionMark;

        private string GetOptionalUnion()
        {
            switch (_optionalFieldRendering)
            {
                case OptionalFieldRendering.WithNullUnion:
                    return $" | null";
                case OptionalFieldRendering.WithUndefinedUnion:
                    return $" | undefined";
                default:
                    return string.Empty;
            }
        }

        public CodeSnippetWriter Build(IComplexTypeInfo typeInfo)
        {
            return writer =>
            {
                writer.Write($"export interface ");
                writer.WriteTypeDec(typeInfo.Name, typeInfo.GenericTypeParams);
                writer.OpenBlock();
                foreach (var prop in typeInfo.Properties)
                {
                    writer.Write($"{prop.Name}{(prop.Type.Optional && UseQuestionMark ? "?" : "")}: ", true);
                    writer.WriteTypeRef(prop.Type);
                    if (prop.Type.Optional)
                        writer.Write(GetOptionalUnion());
                    writer.WriteLine(";", false);
                }

                foreach (var method in typeInfo.Methods)
                {
                    writer.Write(method.IsStatic ? "static " : string.Empty, true);
                    writer.Write($"{method.Name}(");
                    writer.WriteDelimited(method.Params,
                        (p, w) =>
                        {
                            w.Write($"{p.Name}: ");
                            w.WriteTypeRef(p.Type);
                        }, ", ");

                    writer.Write("): ");
                    writer.WriteTypeRef(method.ReturnType);
                    writer.WriteLine(";", false);
                }

                writer.CloseBlock();
            };
        }

    }
}
