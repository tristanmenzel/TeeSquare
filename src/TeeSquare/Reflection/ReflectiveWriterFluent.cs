using System;
using System.Collections.Generic;
using System.IO;
using TeeSquare.Writers;

namespace TeeSquare.Reflection
{
    public class ReflectiveWriterFluent
    {
        private readonly ReflectiveWriterOptions _options;
        private readonly List<Type> _types;

        public ReflectiveWriterFluent()
        {
            _options = new ReflectiveWriterOptions();
            _types = new List<Type>();
        }

        public ReflectiveWriterFluent Configure(Action<IReflectiveWriterOptions> configure)
        {
            configure(_options);
            return this;
        }


        public ReflectiveWriterFluent AddImportedTypes(params (string path, Type[] types)[] importedTypes)
        {
            foreach (var (path, types) in importedTypes)
            foreach (var type in types)
            {
                _options.Types.AddImported(path, type);
            }
            return this;
        }

        public ReflectiveWriterFluent AddTypes(params Type[] types)
        {
            _types.AddRange(types);
            return this;
        }

        public void WriteToFile(string path)
        {
            using (var f = File.Open(path, FileMode.Create))
            {
                WriteToStream(f);
            }
        }

        public void WriteToStream(Stream stream)
        {
            var tsWriter = new TypeScriptWriter(stream, _options);
            var rWriter = new ReflectiveWriter(_options);
            rWriter.AddTypes(_types.ToArray());
            rWriter.WriteTo(tsWriter);
        }

        public string WriteToString()
        {
            using (var ms = new MemoryStream())
            {
                WriteToStream(ms);
                var res = ms.ToArray();
                using (var reader = new StreamReader(new MemoryStream(res)))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
