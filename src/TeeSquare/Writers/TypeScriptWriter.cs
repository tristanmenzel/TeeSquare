using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class TypeScriptWriter : IDisposable
    {
        private readonly IInterfaceWriterFactory _interfaceWriterFactory;
        private readonly IClassWriterFactory _classWriterFactory;
        private readonly IEnumWriterFactory _enumWriterFactory;
        private readonly IFunctionWriterFactory _functionWriterFactory;
        private readonly List<CodeSnippetWriter> _parts;

        private int _cursor = 0;
        private readonly ICodeWriter _codeWriter;

        public TypeScriptWriter(Stream stream,
            IInterfaceWriterFactory interfaceWriterFactory,
            IClassWriterFactory classWriterFactory,
            IEnumWriterFactory enumWriterFactory,
            IFunctionWriterFactory functionWriterFactory,
            string indentCharacters = "  ")
        {
            _interfaceWriterFactory = interfaceWriterFactory;
            _classWriterFactory = classWriterFactory;
            _enumWriterFactory = enumWriterFactory;
            _functionWriterFactory = functionWriterFactory;
            _codeWriter = new CodeWriter(stream, indentCharacters);
            _parts = new List<CodeSnippetWriter>();
        }

        public IComplexTypeConfigurator WriteInterface(string name, params ITypeReference[] genericTypeParams)
        {
            var type = new ComplexTypeInfo(name, genericTypeParams);
            var part = _interfaceWriterFactory.Build(type);
            _parts.Add(part);
            return type;
        }

        public void WriteInterface(IComplexTypeInfo type)
        {
            var part = _interfaceWriterFactory.Build(type);
            _parts.Add(part);
        }

        public void WriteComment(string commentText)
        {
            _parts.Add(writer => writer.WriteLine($"// {commentText}"));
        }


        public IComplexTypeConfigurator WriteClass(string name, params ITypeReference[] genericTypeParams)
        {
            var type = new ComplexTypeInfo(name, genericTypeParams);
            var part = _classWriterFactory.Build(type);
            _parts.Add(part);
            return type;
        }

        public void WriteClass(IComplexTypeInfo type)
        {
            var part = _classWriterFactory.Build(type);
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
            var part = _functionWriterFactory.Build(methodInfo);
            _parts.Add(part);
            return methodInfo;
        }

        public IEnumConfigurator WriteEnum(string name, EnumValueType valueType = EnumValueType.Number)
        {
            var enumInfo = new EnumInfo(name, valueType);
            var part = _enumWriterFactory.Build(enumInfo);
            _parts.Add(part);
            return enumInfo;
        }

        public void WriteLine(string line = "")
        {
            _parts.Add(writer => writer.WriteLine(line));
        }
    }
}
