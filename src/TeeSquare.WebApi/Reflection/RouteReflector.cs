using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    
    public class RouteReflectorOptions
    {
        public WriterOptions BuildWriterOptions()
        {
            return new WriterOptions
            {
                Namer = Namer,
                PropertyFlags = PropertyFlags,
                WriteEnumDescriptions = WriteEnumDescriptions,
                WriteEnumDescriptionGetters = WriteEnumDescriptionGetters,
                IndentChars = IndentChars,
                WriteEnumAllValuesConst = WriteEnumAllValuesConst,
                CustomEnumWriter = CustomEnumWriter,
                CustomInterfaceWriter = CustomInterfaceWriter,
                DiscriminatorPropertyPredicate = DiscriminatorPropertyPredicate,
                DiscriminatorPropertyValueProvider = DiscriminatorPropertyValueProvider
            };
        }
        
        public RouteNamer Namer { get; set; } = new RouteNamer();

        public BindingFlags PropertyFlags { get; set; } = BindingFlags.GetProperty
                                                          | BindingFlags.Public
                                                          | BindingFlags.Instance;

        public bool WriteEnumDescriptions { get; set; }
        public bool WriteEnumDescriptionGetters { get; set; }
        public string IndentChars { get; set; } = "  ";
        public bool WriteEnumAllValuesConst { get; set; }

        public Action<IEnumInfo, ICodeWriter> CustomEnumWriter { get; set; }

        public Action<ITypeInfo, ICodeWriter> CustomInterfaceWriter { get; set; }

        public DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get; set; } = WriterOptions.DefaultDescriminator;
        public DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get; set; } = WriterOptions.DefaultDescriminatorValueProvider;
    }
    
    public class RouteReflector
    {
        private readonly RouteReflectorOptions _options;

        private readonly List<RequestInfo> _requests;


        public RouteReflector(RouteReflectorOptions options)
        {
            _options = options;
            _requests = new List<RequestInfo>();
        }

        public void AddAssembly(Assembly assembly)
        {
            var controllers = assembly.GetExportedTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t));

            foreach (var controller in controllers)
            {
                foreach (var action in controller
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod)
                    .Where(a => a.IsAction()))
                {
                    var route = BuildRoute(controller, action);

                    var factory = GetRequestFactory(action);
                    var requestParams = GetRequestParams(action);


                    _requests.Add(factory(_options.Namer.RouteName(controller, action, route),
                        route,
                        action.ReturnType,
                        requestParams
                    ));
                }
            }
        }

        private ParamInfo[] GetRequestParams(MethodInfo action)
        {
            return action.GetParameters()
                .Select(p => new ParamInfo
                {
                    Kind = GetParameterKind(p),
                    Name = p.Name,
                    Type = p.ParameterType
                }).ToArray();
        }

        public ParameterKind GetParameterKind(ParameterInfo parameterInfo)
        {
            if (parameterInfo.GetCustomAttributes<FromBodyAttribute>().Any())
                return ParameterKind.Body;
            if (parameterInfo.GetCustomAttributes<FromQueryAttribute>().Any())
                return ParameterKind.Query;
            if (parameterInfo.GetCustomAttributes<FromRouteAttribute>().Any())
                return ParameterKind.Route;
            // TODO: Check if in route, else use query
            return ParameterKind.Route;
        }

        private RequestFactory GetRequestFactory(MethodInfo action)
        {
            if (action.GetCustomAttributes<HttpPutAttribute>().Any())
                return RequestInfo.Put;
            if (action.GetCustomAttributes<HttpPostAttribute>().Any())
                return RequestInfo.Post;
            if (action.GetCustomAttributes<HttpDeleteAttribute>().Any())
                return RequestInfo.Delete;
            return RequestInfo.Get;
        }

        private string BuildRoute(Type controller, MethodInfo action)
        {
            var controllerRouteTemplate = controller.GetCustomAttributes<RouteAttribute>()
                .Select(r => r.Template)
                .FirstOrDefault();
            var methodRouteTemplate = action.GetCustomAttributes<RouteAttribute>()
                .Select(r => r.Template)
                .FirstOrDefault();
            var httpMethodTemplate = action.GetCustomAttributes()
                .OfType<HttpMethodAttribute>()
                .Select(r => r.Template)
                .FirstOrDefault();

            var parts = new List<string>();

            if (methodRouteTemplate?.StartsWith("/") != true && controllerRouteTemplate != null)
                parts.Add(controllerRouteTemplate);

            if (methodRouteTemplate != null)
                parts.Add(methodRouteTemplate);
            if (httpMethodTemplate != null)
            {
                if (httpMethodTemplate.StartsWith("/"))
                    parts = new List<string> {httpMethodTemplate};
                else
                    parts.Add(httpMethodTemplate);
            }


            return string.Join("/", parts)
                .Replace("[controller]", controller.Name.Replace("Controller", "").ToLower())
                .Trim('/');
        }

        public void WriteTo(TypeScriptWriter writer)
        {
            writer.WriteInterface("GetRequest", "TResponse")
                .With(i =>
                {
                    i.Property("url", "string");
                    i.Property("method", "'GET'");
                });
            writer.WriteInterface("DeleteRequest", "TResponse")
                .With(i =>
                {
                    i.Property("url", "string");
                    i.Property("method", "'DELETE'");
                });

            writer.WriteInterface("PostRequest", "TRequest", "TResponse")
                .With(i =>
                {
                    i.Property("data", "TRequest");
                    i.Property("url", "string");
                    i.Property("method", "'POST'");
                });
            writer.WriteInterface("PutRequest", "TRequest", "TResponse")
                .With(i =>
                {
                    i.Property("data", "TRequest");
                    i.Property("url", "string");
                    i.Property("method", "'PUT'");
                });


            writer.WriteClass("RequestFactory")
                .Abstract()
                .With(c =>
                {
                    c.Method("toQuery")
                        .WithReturnType("string")
                        .WithParams(p => p.Param("o", "{[key: string]: any}"))
                        .Static()
                        .WithBody(w =>
                        {
                            w.WriteLine("let q = Object.keys(o)");
                            w.Indent();
                            w.WriteLine(".map(k => ({k, v: o[k]}))");
                            w.WriteLine(".filter(x => x.v !== undefined && x.v === null)");
                            w.WriteLine(".map(x => `${encodeURIComponent(x.k)}=${encodeURIComponent(x.v)}`)");
                            w.WriteLine(".join('&');");
                            w.Deindent();
                            w.WriteLine("return q && `?${q}` || '';");
                        });


                    foreach (var req in _requests)
                    {
                        var methodBuilder = c.Method($"{req.Method.GetName()}{req.Name}")
                            .Static();

                        if (req.Method.HasRequestBody())
                        {
                            methodBuilder
                                .WithReturnType($"{req.Method.GetName()}Request",
                                    _options.Namer.TypeName(req.GetRequestBodyType()),
                                    _options.Namer.TypeName(req.ResponseType));
                        }
                        else
                        {
                            methodBuilder
                                .WithReturnType($"{req.Method.GetName()}Request",
                                    _options.Namer.TypeName(req.ResponseType));
                        }

                        methodBuilder.WithParams(p =>
                            {
                                foreach (var rp in req.RequestParams.Where(x => x.Kind != ParameterKind.Body))

                                {
                                    if (rp.Type.IsNullable(out var underlyingType))
                                    {
                                        p.Param(rp.Name + "?", _options.Namer.TypeName(underlyingType));
                                        continue;
                                    }

                                    p.Param(rp.Name, _options.Namer.TypeName(rp.Type));
                                }

                                if (req.Method.HasRequestBody())
                                    p.Param("data", _options.Namer.TypeName(req.GetRequestBodyType()));
                            })
                            .WithBody(w =>
                            {
                                var queryParams = req.RequestParams.Where(x => x.Kind == ParameterKind.Query).ToArray();
                                if (queryParams.Any())
                                {
                                    w.Write("let query = RequestFactory.toQuery({", true);
                                    w.WriteDelimited(queryParams,
                                        (p, wr) => wr.Write(p.Name), ", ");
                                    w.WriteLine("});", false);
                                }

                                w.WriteLine("return {");
                                w.Indent();
                                w.WriteLine($"method: '{req.Method.GetName().ToUpper()}',");
                                if (req.Method.HasRequestBody())
                                    w.WriteLine("data,");
                                w.WriteLine(
                                    $"url: `{req.Path.Replace("{", "${")}{(queryParams.Any() ? "${query}" : "")}`");
                                w.Deindent();
                                w.WriteLine("};");
                            });
                    }
                });
            var types = _requests.Select(r => r.ResponseType)
                .Union(_requests.SelectMany(r => r.RequestParams.Select(p => p.Type)));

            var rWriter = new ReflectiveWriter(_options.BuildWriterOptions());
            rWriter.AddTypes(types.ToArray());
            rWriter.WriteTo(writer);
        }
    }

    public enum ParameterKind
    {
        Route,
        Query,
        Body
    }

    public class ParamInfo
    {
        public ParameterKind Kind { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }

    public class RequestInfo
    {
        private RequestInfo(string name, HttpMethod method, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            Name = name;
            Method = method;
            Path = path;
            ResponseType = responseType;
            RequestParams = requestParams;
        }

        public string Name { get; }
        public HttpMethod Method { get; }
        public string Path { get; }
        public Type ResponseType { get; }
        public ParamInfo[] RequestParams { get; }

        public Type GetRequestBodyType()
        {
            return RequestParams
                       .Where(p => p.Kind == ParameterKind.Body)
                       .Select(p => p.Type)
                       .FirstOrDefault() ?? typeof(void);
        }

        public static RequestInfo Post(string name, string path, Type responseType, ParamInfo[] requestParams)
        {
            return new RequestInfo(name, HttpMethod.Post, path, responseType, requestParams);
        }

        public static RequestInfo Get(string name, string path, Type responseType, ParamInfo[] requestParams)
        {
            return new RequestInfo(name, HttpMethod.Get, path, responseType, requestParams);
        }

        public static RequestInfo Put(string name, string path, Type responseType, ParamInfo[] requestParams)
        {
            return new RequestInfo(name, HttpMethod.Put, path, responseType, requestParams);
        }

        public static RequestInfo Delete(string name, string path, Type responseType, ParamInfo[] requestParams)
        {
            return new RequestInfo(name, HttpMethod.Delete, path, responseType, requestParams);
        }
    }

    public delegate RequestInfo RequestFactory(string name, string path, Type responseType,
        ParamInfo[] requestParams);

    public class RouteNamer : Namer
    {
        public override bool HasStaticMapping(Type type)
        {
            if (type == typeof(IActionResult))
                return true;
            return base.HasStaticMapping(type);
        }

        public override bool TryGetStaticMapping(Type type, out string name)
        {
            if (type == typeof(IActionResult))
            {
                name = "any";
                return true;
            }

            return base.TryGetStaticMapping(type, out name);
        }

        public virtual string RouteName(Type controller, MethodInfo action, string route)
        {
            var parts = route.Split('/')
                .Select(part =>
                {
                    if (part.StartsWith("{"))
                    {
                        return $"By{ToCase(part.Substring(1, part.Length - 2), NameConvention.PascalCase)}";
                    }

                    return ToCase(part, NameConvention.PascalCase);
                });
            return string.Join("", parts).Replace("[controller]", ControllerName(controller));
        }

        public string ControllerName(Type controller)
        {
            return controller.Name.Replace("Controller", "");
        }
    }

    public static class WebApiTypeExtension
    {
        public static bool IsAction(this MethodInfo action)
        {
            if (action.IsDefined(typeof(NonActionAttribute))
                || action.IsSpecialName
                || action.DeclaringType.Namespace.StartsWith("System")
                || action.DeclaringType.Namespace.StartsWith("Microsoft"))
                return false;
            return true;
        }
    }

    public static class HttpMethodExtensions
    {
        public static bool HasRequestBody(this HttpMethod method)
        {
            return method == HttpMethod.Put || method == HttpMethod.Post;
        }

        public static string GetName(this HttpMethod method)
        {
            if (method == HttpMethod.Delete)
                return "Delete";

            if (method == HttpMethod.Post)
                return "Post";

            if (method == HttpMethod.Put)
                return "Put";

            return "Get";
        }
    }
}