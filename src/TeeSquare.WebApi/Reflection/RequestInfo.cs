using System;
using System.Linq;
using System.Net.Http;

namespace TeeSquare.WebApi.Reflection
{
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
}