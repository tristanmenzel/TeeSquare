using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Util;
using TeeSquare.Writers;

namespace TeeSquare.WebApi.Reflection
{
    public class RouteReflector
    {
        private readonly IRouteReflectorOptions _options;

        private readonly List<RequestInfo> _requests = new List<RequestInfo>();

        private readonly List<Type> _additionalTypes = new List<Type>();

        public RouteReflector(IRouteReflectorOptions options)
        {
            _options = options;
        }

        public void AddAdditionalTypes(Type[] types)
        {
            _additionalTypes.AddRange(types);
        }

        public void AddAssembly(Assembly assembly, Func<Type, bool> controllerFilter)
        {
            CheckConfigured();
            var controllers = assembly.GetExportedTypes()
                .Where(t => StaticConfig.Instance.ControllerType.IsAssignableFrom(t))
                .Where(controllerFilter);

            foreach (var controller in controllers)
            {
                AddController(controller);
            }
        }

        public void AddController(Type controller)
        {
            foreach (var action in controller
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod)
                .Where(a => a.IsAction()))
            {
                var route = _options.BuildRouteStrategy(controller, action, _options.DefaultRoute);

                var (factory, method) = _options.GetHttpMethodAndRequestFactoryStrategy(controller, action);
                var requestParams = GetRequestParams(action, route, method);

                var returnType = _options.GetApiReturnTypeStrategy(controller, action);
                _requests.Add(factory(_options.FactoryNameStrategy(controller, action, route),
                    _options.NameRouteStrategy(controller, action, route, method),
                    route,
                    returnType,
                    requestParams
                ));
            }
        }

        internal static Type DefaultApiReturnTypeStrategy(Type controller, MethodInfo action)
        {
            return action.ReturnType;
        }

        private ParamInfo[] GetRequestParams(MethodInfo action, string route, HttpMethod method)
        {
            return action.GetParameters()
                .Select(p =>
                {
                    var kind = _options.GetParameterKindStrategy(p, route, method);
                    return new ParamInfo
                    {
                        Kind = kind,
                        Name = p.Name,
                        Type = p.ParameterType,
                        DestructuredProperties = kind == ParameterKind.Route
                            ? GetDestructuredProperties(p, route)
                            : Array.Empty<DestructuredPropertyInfo>()
                    };
                }).ToArray();
        }

        private DestructuredPropertyInfo[] GetDestructuredProperties(ParameterInfo parameterInfo, string route)
        {
            if (parameterInfo.ParameterType.IsExtendedPrimitive())
            {
                return Array.Empty<DestructuredPropertyInfo>();
            }

            var props = parameterInfo.ParameterType.GetProperties()
                .Select(p => _options.TypeConverter.PropertyName(p))
                .ToArray();
            var routeParams = new Regex("{([^}]+)}").Matches(route).Cast<Match>()
                .Select(m => m.Groups[1].Value).ToArray();
            return props
                .SelectMany(p =>
                {
                    var matchedRouteParam =
                        routeParams.FirstOrDefault(rp => rp.Equals(p, StringComparison.InvariantCultureIgnoreCase));
                    if (matchedRouteParam != default)
                    {
                        return new[]
                        {
                            new DestructuredPropertyInfo {DestructureAs = matchedRouteParam, PropertyName = p}
                        };
                    }

                    return Array.Empty<DestructuredPropertyInfo>();
                })
                .ToArray();
        }

        internal static ParameterKind GetParameterKind(ParameterInfo parameterInfo, string route, HttpMethod method)
        {
            if (parameterInfo.GetCustomAttributes(StaticConfig.Instance.FromBodyAttribute).Any())
                return ParameterKind.Body;
            if (parameterInfo.GetCustomAttributes(StaticConfig.Instance.FromQueryAttribute).Any())
                return ParameterKind.Query;
            if (parameterInfo.GetCustomAttributes(StaticConfig.Instance.FromRouteAttribute).Any())
                return ParameterKind.Route;
            if (parameterInfo.GetCustomAttributes(StaticConfig.Instance.FromFormAttribute).Any())
                return ParameterKind.Form;

            if (route.Contains($"{{{parameterInfo.Name}}}"))
            {
                return ParameterKind.Route;
            }

            if ((method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch)
                && IsPossibleDto(parameterInfo.ParameterType))
            {
                return ParameterKind.Body;
            }

            return ParameterKind.Query;
        }

        private static bool IsPossibleDto(Type type)
        {
            return !type.IsPrimitive
                   && type != typeof(string)
                   && !type.IsEnum
                   && !type.IsValueType;
        }

        internal static (RequestFactory factory, HttpMethod method) DefaultGetHttpMethodAndRequestFactory(
            Type controller, MethodInfo action)
        {
            if (action.GetCustomAttributes(StaticConfig.Instance.HttpPutAttribute).Any())
                return (RequestInfo.Put, HttpMethod.Put);
            if (action.GetCustomAttributes(StaticConfig.Instance.HttpPostAttribute).Any())
                return (RequestInfo.Post, HttpMethod.Post);
            if (action.GetCustomAttributes(StaticConfig.Instance.HttpPatchAttribute).Any())
                return (RequestInfo.Patch, HttpMethod.Patch);
            if (action.GetCustomAttributes(StaticConfig.Instance.HttpOptionsAttribute).Any())
                return (RequestInfo.Options, HttpMethod.Options);
            if (action.GetCustomAttributes(StaticConfig.Instance.HttpDeleteAttribute).Any())
                return (RequestInfo.Delete, HttpMethod.Delete);
            return (RequestInfo.Get, HttpMethod.Get);
        }

        internal static string DefaultNameRouteStrategy(Type controller, MethodInfo action, string route,
            HttpMethod method)
        {
            var parts = route.Split('/')
                .Select(part =>
                {
                    if (part.StartsWith("{"))
                    {
                        return $"By{CaseHelper.ToCase(part.Substring(1, part.Length - 2), NameConvention.PascalCase)}";
                    }

                    return CaseHelper.ToCase(part, NameConvention.PascalCase);
                });
            return method.GetName() + string.Join("", parts);
        }

        internal static string DefaultBuildRouteStrategy(Type controller, MethodInfo action, string defaultRoute)
        {
            var controllerRouteTemplate = controller.GetCustomAttributes(StaticConfig.Instance.RouteAttribute)
                .Select(StaticConfig.Instance.GetTemplateFromRouteAttribute)
                .FirstOrDefault();
            var methodRouteTemplate = action.GetCustomAttributes(StaticConfig.Instance.RouteAttribute)
                .Select(StaticConfig.Instance.GetTemplateFromRouteAttribute)
                .FirstOrDefault();
            var httpMethodTemplate = action.GetCustomAttributes()
                .Where(a => StaticConfig.Instance.HttpMethodBaseAttribute.IsInstanceOfType(a))
                .Select(StaticConfig.Instance.GetTemplateFromHttpMethodAttribute)
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

            var routeTemplate = parts.Any() ? string.Join("/", parts) : defaultRoute;


            return ReplaceRoutePlaceholders(routeTemplate, controller, action)
                .Trim('/');
        }

        public static string ReplaceRoutePlaceholders(string routeTemplate, Type controller, MethodInfo action)
        {
            var partRegexSquare = new Regex(@"\[(?<part>[^]=?]+)[?]?(=[^]]+)?\]");
            var partRegexBraces = new Regex(@"\{(?<part>[^}=?]+)[?]?(=[^}]+)?\}");

            string extractParameterName(string part) =>
                !part.Contains(':') ? part : part.Substring(0, part.IndexOf(':'));

            bool doesRouteParamExist(string paramName, MethodInfo action)
            {
                if (action.GetParameters()
                    .Any(p => p.Name.Equals(paramName, StringComparison.InvariantCultureIgnoreCase)))
                    return true;
                if (action.GetParameters()
                    .Where(p => !p.ParameterType.IsExtendedPrimitive()
                                && p.GetCustomAttributes(StaticConfig.Instance.FromRouteAttribute).Any())
                    .Any(p => p.ParameterType.GetProperties().Any(p =>
                        p.Name.Equals(paramName, StringComparison.InvariantCultureIgnoreCase))))
                    return true;
                return false;
            }

            string MatchReplacer(Match match) => match.Groups["part"].Value switch
            {
                "controller" => controller.Name.Replace("Controller", "").ToLower(),
                "action" => action.Name.ToLower(),
                _ => doesRouteParamExist(extractParameterName(match.Groups["part"].Value), action)
                    ? $"{{{extractParameterName(match.Groups["part"].Value)}}}"
                    : ""
            };


            return partRegexSquare.Replace(
                partRegexBraces.Replace(routeTemplate, MatchReplacer),
                MatchReplacer);
        }


        private void WriteRequestTypesAndHelpers(TypeScriptWriter writer)
        {
            writer.WriteInterface("GetRequest", new TypeReference("TResponse"))
                .Configure(i =>
                {
                    i.AddProperty("url", new TypeReference("string"));
                    i.AddProperty("method", new TypeReference("'GET'"));
                });
            writer.WriteInterface("OptionsRequest", new TypeReference("TResponse"))
                .Configure(i =>
                {
                    i.AddProperty("url", new TypeReference("string"));
                    i.AddProperty("method", new TypeReference("'OPTIONS'"));
                });
            writer.WriteInterface("DeleteRequest", new TypeReference("TResponse"))
                .Configure(i =>
                {
                    i.AddProperty("url", new TypeReference("string"));
                    i.AddProperty("method", new TypeReference("'DELETE'"));
                });
            writer.WriteInterface("PostRequest", new TypeReference("TRequest"), new TypeReference("TResponse"))
                .Configure(i =>
                {
                    i.AddProperty("data", new TypeReference("TRequest"));
                    i.AddProperty("url", new TypeReference("string"));
                    i.AddProperty("method", new TypeReference("'POST'"));
                });
            writer.WriteInterface("PatchRequest", new TypeReference("TRequest"), new TypeReference("TResponse"))
                .Configure(i =>
                {
                    i.AddProperty("data", new TypeReference("TRequest"));
                    i.AddProperty("url", new TypeReference("string"));
                    i.AddProperty("method", new TypeReference("'PATCH'"));
                });
            writer.WriteInterface("PutRequest", new TypeReference("TRequest"), new TypeReference("TResponse"))
                .Configure(i =>
                {
                    i.AddProperty("data", new TypeReference("TRequest"));
                    i.AddProperty("url", new TypeReference("string"));
                    i.AddProperty("method", new TypeReference("'PUT'"));
                });
            writer.WriteFunction("toQuery")
                .WithReturnType(new TypeReference("string"))
                .WithParams(p => p.Param("o", new TypeReference("{[key: string]: any}")))
                .Static()
                .WithBody(w =>
                {
                    w.WriteLine("const q = Object.keys(o)");
                    w.Indent();
                    w.WriteLine(".map(k => ({k, v: o[k]}))");
                    w.WriteLine(".filter(x => x.v !== undefined && x.v !== null)");
                    w.WriteLine(".map(x => Array.isArray(x.v)");
                    w.Indent();
                    w.WriteLine("? x.v.map(v => `${encodeURIComponent(x.k)}=${encodeURIComponent(v)}`).join('&')");
                    w.WriteLine(": `${encodeURIComponent(x.k)}=${encodeURIComponent(x.v)}`)");
                    w.Deindent();
                    w.WriteLine(".join('&');");
                    w.Deindent();
                    w.WriteLine("return q ? `?${q}` : '';");
                });
        }

        private void CheckConfigured()
        {
            if (StaticConfig.Instance.ControllerType == null)
            {
                throw new Exception(
                    "WebApi reflector has not been configured. Please include a TeeSquare.WebApi.{Platform} library or provide a manual configuration.");
            }
        }

        public void WriteTo(TypeScriptWriter writer)
        {
            CheckConfigured();
            _options.WriteHeader(writer);

            var rWriter = new ReflectiveWriter(_options);

            if (!_options.RequestHelperTypeOption.ShouldEmitTypes)
            {
                foreach (var type in _options.RequestHelperTypeOption.Types)
                    _options.Types.AddLiteralImport(_options.RequestHelperTypeOption.ImportFrom, type);
            }

            AddTypeDependencies(rWriter);

            rWriter.WriteImports(writer);


            if (_options.RequestHelperTypeOption.ShouldEmitTypes)
            {
                WriteRequestTypesAndHelpers(writer);
            }

            foreach (var factory in _requests.GroupBy(r => r.FactoryName))
            {
                writer.WriteClass(factory.Key)
                    .Configure(c =>
                    {
                        c.MakeAbstract();


                        foreach (var req in factory)
                        {
                            var methodBuilder = c.AddMethod($"{req.Name}")
                                .Static();

                            if (req.Method.HasRequestBody())
                            {
                                var requestBodyType = req.GetRequestBodyKind() == RequestBodyKind.Json
                                    ? _options.TypeConverter.Convert(req.GetRequestBodyType(), null)
                                    : (req.GetRequestBodyKind() == RequestBodyKind.FormData
                                        ? new TypeReference("FormData")
                                        : _options.EmptyRequestBodyType);
                                if (requestBodyType.Optional)
                                {
                                    requestBodyType = new TypeReference($"{requestBodyType.FullName} | undefined");
                                }

                                methodBuilder
                                    .WithReturnType(new TypeReference($"{req.Method.GetName()}Request",
                                        new[]
                                        {
                                            requestBodyType,
                                            _options.TypeConverter.Convert(req.ResponseType, null)
                                        }));
                            }
                            else
                            {
                                methodBuilder
                                    .WithReturnType(new TypeReference($"{req.Method.GetName()}Request",
                                        new[]
                                        {
                                            _options.TypeConverter.Convert(req.ResponseType, null)
                                        }));
                            }

                            methodBuilder.WithParams(p =>
                                {
                                    foreach (var rp in req.RequestParams.Where(x => x.Kind != ParameterKind.Body))

                                    {
                                        p.Param(rp.NameOrDestructureExpression,
                                            _options.TypeConverter.Convert(rp.Type)
                                                .MakeOptional(rp.Kind == ParameterKind.Query &&
                                                              rp.Type.IsExtendedPrimitive(true))
                                                .MakePartial(rp.Kind == ParameterKind.Query &&
                                                             !rp.Type.IsExtendedPrimitive(true)));
                                    }

                                    if (req.Method.HasRequestBody() && req.GetRequestBodyKind() == RequestBodyKind.Json)
                                        p.Param("data", _options.TypeConverter.Convert(req.GetRequestBodyType(), null));
                                })
                                .WithBody(w =>
                                {
                                    var queryParams = req.RequestParams.Where(x => x.Kind == ParameterKind.Query)
                                        .ToArray();
                                    if (queryParams.Any())
                                    {
                                        w.Write($"const {_options.QueryVariableName} = toQuery({{ ", true);
                                        w.WriteDelimited(queryParams,
                                            (p, wr) => wr.Write(p.Type.IsExtendedPrimitive(true) ? p.Name : $"...{p.Name}"),
                                            ", ");
                                        w.WriteLine(" });", false);
                                    }

                                    var formParams = req.RequestParams.Where(x => x.Kind == ParameterKind.Form)
                                        .ToArray();
                                    if ((formParams.Any()))
                                    {
                                        w.WriteLine("const data = new FormData();");
                                        foreach (var formParam in formParams)
                                        {
                                            w.WriteLine($"data.append('{formParam.Name}', {formParam.Name});");
                                        }
                                    }

                                    w.WriteLine("return {");
                                    w.Indent();
                                    w.WriteLine($"method: '{req.Method.GetName().ToUpper()}',");
                                    if (req.Method.HasRequestBody() &&
                                        req.GetRequestBodyKind() != RequestBodyKind.Empty)
                                        w.WriteLine("data,");
                                    else if (req.Method.HasRequestBody())
                                        w.WriteLine("data: undefined,");
                                    w.WriteLine(
                                        $"url: `{req.Path.Replace("{", "${")}{(queryParams.Any() ? $"${{{_options.QueryVariableName}}}" : "")}`");
                                    w.Deindent();
                                    w.WriteLine("};");
                                });
                        }
                    });
            }


            rWriter.WriteTo(writer, false);
        }

        private void AddTypeDependencies(ReflectiveWriter rWriter)
        {
            var types = _requests.Select(r => r.ResponseType)
                .Union(_requests.SelectMany(r => r.RequestParams.Select(p => p.Type)))
                .Union(_additionalTypes);

            rWriter.AddTypes(types.ToArray());
        }

        public static string DefaultFactoryNameStrategy(Type controller, MethodInfo action, string route)
        {
            return "RequestFactory";
        }
    }
}
