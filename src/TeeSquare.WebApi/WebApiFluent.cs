using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TeeSquare.WebApi.Reflection;
using TeeSquare.Writers;

namespace TeeSquare.WebApi
{
    public class WebApiFluent
    {
        private readonly Type[] _controllers;
        private readonly List<Type[]> _types = new List<Type[]>();
        private readonly Assembly[] _assemblies;
        private readonly RouteReflectorOptions _options;

        public WebApiFluent(Assembly[] assemblies)
        {
            _assemblies = assemblies;
            _options = new RouteReflectorOptions();
        }

        public WebApiFluent(Type[] controllers)
        {
            _controllers = controllers;
            _options = new RouteReflectorOptions();
        }

        public WebApiFluent Configure(Action<RouteReflectorOptions> configure)
        {
            configure(_options);
            return this;
        }

        public WebApiFluent AddTypes(params Type[] types)
        {
            _types.Add(types);
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
            var tsWriter = new TypeScriptWriter(stream,_options);

            var webApiWriter = new RouteReflector(_options);

            foreach (var additionalTypes in _types)
                webApiWriter.AddAdditionalTypes(additionalTypes);

            foreach (var assembly in _assemblies ?? Array.Empty<Assembly>())
                webApiWriter.AddAssembly(assembly);
            foreach (var controller in _controllers ?? Array.Empty<Type>())
                webApiWriter.AddController(controller);

            webApiWriter.WriteTo(tsWriter);
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
