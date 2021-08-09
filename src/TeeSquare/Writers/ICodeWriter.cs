using System;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public interface ICodeWriter : IDisposable
    {
        void Indent();
        void Deindent();
        void Write(string text, bool indent = false);
        void WriteLine(string text, bool indent = true);
        void WriteDelimitedLines<T>(T[] items, Func<T, string> lineFunc, string delimiter);
        void WriteDelimited<T>(T[] items, Action<T, ICodeWriter> lineFunc, string delimiter);
        void WriteTypeRef(ITypeReference typeReference);
        void WriteTypeDec(string typeName, ITypeReference[] genericTypeParams);
        void OpenBlock(string? text = null, string openBlockDelimiter = "{");
        void CloseBlock(string closeBlockDelimiter = "}");

        void Flush();
    }
}
