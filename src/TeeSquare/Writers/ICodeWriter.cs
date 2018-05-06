using System;
using System.Collections.Generic;

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
        void WriteType(string type, string[] typeParams);
        void OpenBrace(string text =null);
        void CloseBrace();

        void Flush();
    }
}
