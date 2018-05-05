using System;

namespace TeeSquare.Writers
{
    public interface ICodeWriter : IDisposable
    {
        void Indent();
        void Deindent();
        void Write(string text, bool indent = false);
        void WriteLine(string text, bool indent = true);
        void WriteType(string type, string[] typeParams);
        void OpenBrace();
        void CloseBrace();

        void Flush();
    }
}