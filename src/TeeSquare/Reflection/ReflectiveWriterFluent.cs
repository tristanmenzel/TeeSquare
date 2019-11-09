using System;
using System.Collections.Generic;
using System.IO;
using TeeSquare.Writers;

namespace TeeSquare.Reflection
{
    public class ReflectiveWriterFluent
    {
        private readonly WriterOptions _options;
        private readonly List<Type> _types;

        public ReflectiveWriterFluent()
        {
            _options = new WriterOptions();
            _types = new List<Type>();
        }

        public ReflectiveWriterFluent Configure(Action<WriterOptions> configure)
        {
            configure(_options);
            return this;
        }

        public ReflectiveWriterFluent AddTypes(params Type[] types)
        {
            _types.AddRange(types);
            return this;
        }

        public void WriteToFile(string path)
        {
            using(var f = File.Open(path, FileMode.Create))
            {
                WriteToStream(f);
            }
        }

        public void WriteToStream(Stream stream)
        {
            var tsWriter = new TypeScriptWriter(stream,
                    _options.InterfaceWriterFactory,
                    _options.ClassWriterFactory,
                    _options.EnumWriterFactory,
                    _options.FunctionWriterFactory,
                    _options.IndentChars);
            var rWriter = new ReflectiveWriter(_options);
            rWriter.AddTypes(_types.ToArray());
            rWriter.WriteTo(tsWriter);
        }

        public string WriteToString()
        {
            using (var ms = new MemoryStream())
            {
                WriteToStream(ms);
                ms.Position = 0;
                var res = ms.ToArray();
                using (var reader = new StreamReader(new MemoryStream(res)))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
