using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Writers
{
    public class TypeScriptWriterFluent
    {
        private List<Action<TypeScriptWriter>> _snippets = new List<Action<TypeScriptWriter>>();
        private TypeScriptWriterOptions _options = new TypeScriptWriterOptions();

        public TypeScriptWriterFluent Configure(Action<TypeScriptWriterOptions> configure)
        {
            configure(_options);
            return this;
        }

        public TypeScriptWriterFluent Add(Action<TypeScriptWriter> snippet)
        {
            _snippets.Add(snippet);
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
                _options);
            _snippets.ForEach(s => s(tsWriter));
            tsWriter.Flush();
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
