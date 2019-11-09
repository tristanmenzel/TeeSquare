using System;
using System.IO;
using System.Linq;
using TeeSquare.TypeMetadata;

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

        public void WriteTypeRef(ITypeReference typeReference)
        {
            _writer.Write(typeReference.FullName);
        }

        public void WriteTypeDec(string typeName, ITypeReference[] genericTypeParams)
        {
            if (genericTypeParams.Any())
                _writer.Write($"{typeName}<{(string.Join(", ", genericTypeParams.Select(t=>t.FullName)))}>");
            else
                _writer.Write($"{typeName}");
        }

        public void OpenBlock(string text = null, string openBlockDelimiter = "{")
        {
            _writer.WriteLine($"{text} {openBlockDelimiter}");
            _indent++;
        }

        public void CloseBlock(string closeBlockDelimiter = "}")
        {
            _indent--;
            WriteLine(closeBlockDelimiter);
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
