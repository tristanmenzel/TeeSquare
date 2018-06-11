using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TeeSquare.Writers
{
    public class CodeWriter : ICodeWriter
    {
        private readonly StreamWriter _writer;
        private readonly string _indentToken;
        private int _indent;

        public CodeWriter(Stream stream, string indentToken)
        {
            _writer = new StreamWriter(stream);
            _indentToken = indentToken;
            _indent = 0;
        }

        public void Indent()
        {
            _indent++;
        }

        public void Deindent()
        {
            _indent--;
        }

        private string CurrentIndent =>
            string.Join(string.Empty, Enumerable.Range(0, _indent).Select(x => _indentToken));

        public void Write(string text, bool indent = false)
        {
            _writer.Write($"{(indent ? CurrentIndent : string.Empty)}{text}");
        }

        public void WriteLine(string text, bool indent = true)
        {
            _writer.WriteLine($"{(indent ? CurrentIndent : string.Empty)}{text}");
        }

        public void WriteDelimitedLines<T>(T[] items, Func<T, string> lineFunc, string delimiter)
        {
            for (var i = 0; i < items.Length; i++)
            {
                var last = i == items.Length - 1;
                WriteLine($"{(lineFunc(items[i]))}{(last ? "" : delimiter)}");
            }
        }

        public void WriteDelimited<T>(T[] items, Action<T, ICodeWriter> writeItem, string delimiter)
        {
            for (var i = 0; i < items.Length; i++)
            {
                var last = i == items.Length - 1;
                writeItem(items[i], this);
                if (!last)
                {
                    Write(delimiter);
                }
            }
        }

        public void WriteType(string type, string[] typeParams)
        {
            if (typeParams.Any())
                _writer.Write($"{type}<{(string.Join(", ", typeParams))}>");
            else
                _writer.Write($"{type}");
        }

        public void OpenBrace(string text = null)
        {
            _writer.WriteLine($"{text} {{");
            _indent++;
        }

        public void CloseBrace(bool semicolon = false)
        {
            _indent--;
            WriteLine(semicolon ? "};" : "}");
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer?.Dispose();
            }
        }
    }
}