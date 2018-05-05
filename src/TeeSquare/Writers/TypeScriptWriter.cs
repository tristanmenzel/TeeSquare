using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TeeSquare.Writers
{
    public class TypeScriptWriter : IDisposable
    {
        private readonly List<ICodePart> _parts;

        private int _cursor = 0;
        private readonly ICodeWriter _codeWriter;

        public TypeScriptWriter(Stream stream, string indentCharacters = "  ")
        {
            _codeWriter = new CodeWriter(stream, indentCharacters);
            _parts = new List<ICodePart>();
        }

        public InterfaceWriter WriteInterface(string name, params string[] genericTypeParams)
        {
            var part = new InterfaceWriter(name, genericTypeParams);
            _parts.Add(part);
            return part;
        }


        public ClassWriter WriteClass(string name, params string[] genericTypeParams)
        {
            var part = new ClassWriter(name, genericTypeParams);
            _parts.Add(part);
            return part;
        }

        public void Flush()
        {
            var parts = _parts.Skip(_cursor).ToArray();
            _cursor += parts.Length;
            foreach (var part in parts)
            {
                part.WriteTo(_codeWriter);
            }

            _codeWriter.Flush();
        }

        public void Dispose()
        {
            Flush();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _codeWriter?.Dispose();
            }
        }
    }
}