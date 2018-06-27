using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.WebApi.Reflection;
using TeeSquare.Writers;

namespace TeeSquare.WebApi
{
    public class TeeSquareWebApi
    {
        public static WebApiFluent GenerateForAssemblies(params Assembly[] assemblies)
        {
            return new WebApiFluent(assemblies);
        }
    }

    public class WebApiFluent
    {
        private readonly Assembly[] _assemblies;
        private RouteReflectorOptions _options;

        public WebApiFluent(Assembly[] assemblies)
        {
            _assemblies = assemblies;
            _options = new RouteReflectorOptions();
        }

        public WebApiFluent Configure(Action<RouteReflectorOptions> configure)
        {
            configure(_options);
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
            var tsWriter = new TypeScriptWriter(stream, _options.IndentChars);

            var webApiWriter = new RouteReflector(_options);
            foreach (var assembly in _assemblies)
                webApiWriter.AddAssembly(assembly);

            webApiWriter.WriteTo(tsWriter);
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