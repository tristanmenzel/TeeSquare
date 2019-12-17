using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class TypeScriptWriter : IDisposable
    {
        private readonly ITypeScriptWriterOptions _options;
        private readonly List<CodeSnippetWriter> _parts;

        private int _cursor = 0;
        private readonly ICodeWriter _codeWriter;

        public TypeScriptWriter(Stream stream,
            ITypeScriptWriterOptions options)
        {
            _options = options ?? new TypeScriptWriterOptions();
            _codeWriter = new CodeWriter(stream, _options.IndentCharacters);
            _parts = new List<CodeSnippetWriter>();
        }

        public IComplexTypeConfigurator WriteInterface(string name, params ITypeReference[] genericTypeParams)
        {
            var type = new ComplexTypeInfo(name, genericTypeParams);
            var part = _options.InterfaceWriterFactory.Build(type);
            _parts.Add(part);
            return type;
        }

        public void WriteInterface(IComplexTypeInfo type)
        {
            var part = _options.InterfaceWriterFactory.Build(type);
            _parts.Add(part);
        }

        public void WriteComment(string commentText)
        {
            _parts.Add(writer => writer.WriteLine($"// {commentText}"));
        }


        public IComplexTypeConfigurator WriteClass(string name, params ITypeReference[] genericTypeParams)
        {
            var type = new ComplexTypeInfo(name, genericTypeParams);
            var part = _options.ClassWriterFactory.Build(type);
            _parts.Add(part);
            return type;
        }

        public void WriteClass(IComplexTypeInfo type)
        {
            var part = _options.ClassWriterFactory.Build(type);
            _parts.Add(part);
        }

        public void WriteSnippet(CodeSnippetWriter snippetWriter)
        {
            _parts.Add(snippetWriter);
        }

        public void Flush()
        {
            var parts = _parts.Skip(_cursor).ToArray();
            _cursor += parts.Length;
            foreach (var part in parts)
            {
                part(_codeWriter);
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

        public IMethodConfigurator WriteFunction(string name, params string[] genericTypeParams)
        {
            var methodInfo = new MethodInfo(name, genericTypeParams);
            var part = _options.FunctionWriterFactory.Build(methodInfo);
            _parts.Add(part);
            return methodInfo;
        }

        public IEnumConfigurator WriteEnum(string name, EnumValueType valueType = EnumValueType.Number)
        {
            var enumInfo = new EnumInfo(name, valueType);
            var part = _options.EnumWriterFactory.Build(enumInfo);
            _parts.Add(part);
            return enumInfo;
        }

        public void WriteLine(string line = "")
        {
            _parts.Add(writer => writer.WriteLine(line));
        }
    }
}
