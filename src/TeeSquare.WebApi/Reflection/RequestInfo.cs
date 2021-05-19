using System;
using System.Linq;

namespace TeeSquare.WebApi.Reflection
{
    public class RequestInfo
    {
        private RequestInfo(string factoryName, string name, HttpMethod method, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            FactoryName = factoryName;
            Name = name;
            Method = method;
            Path = path;
            ResponseType = responseType;
            RequestParams = requestParams;
        }

        public string FactoryName { get; }
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

        public RequestBodyKind GetRequestBodyKind()
        {
            if (RequestParams.Any(rp => rp.Kind == ParameterKind.Form))
                return RequestBodyKind.FormData;
            if (RequestParams.Any(rp => rp.Kind == ParameterKind.Body))
                return RequestBodyKind.Json;
            return RequestBodyKind.Empty;
        }

        public static RequestInfo Post(string factoryName, string name, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            return new RequestInfo(factoryName, name, HttpMethod.Post, path, responseType, requestParams);
        }

        public static RequestInfo Patch(string factoryName, string name, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            return new RequestInfo(factoryName, name, HttpMethod.Patch, path, responseType, requestParams);
        }

        public static RequestInfo Options(string factoryName, string name, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            return new RequestInfo(factoryName, name, HttpMethod.Options, path, responseType, requestParams);
        }

        public static RequestInfo Get(string factoryName, string name, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            return new RequestInfo(factoryName, name, HttpMethod.Get, path, responseType, requestParams);
        }

        public static RequestInfo Put(string factoryName, string name, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            return new RequestInfo(factoryName, name, HttpMethod.Put, path, responseType, requestParams);
        }

        public static RequestInfo Delete(string factoryName, string name, string path, Type responseType,
            ParamInfo[] requestParams)
        {
            return new RequestInfo(factoryName, name, HttpMethod.Delete, path, responseType, requestParams);
        }
    }
}
