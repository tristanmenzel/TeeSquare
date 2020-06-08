using System;

namespace TeeSquare.WebApi.Reflection
{
    public static class StaticConfig
    {
        public static void Configure(IWebApiConfig configuration)
        {
            Instance = configuration;
        }

        public static IWebApiConfig Instance { get; private set; }
    }

    public interface IWebApiConfig
    {
        Type ControllerType { get; }

        Type FromBodyAttribute { get; }
        Type FromQueryAttribute { get; }
        Type FromRouteAttribute { get; }

        Type HttpMethodBaseAttribute { get; }
        Type HttpGetAttribute { get; }
        Type HttpPutAttribute { get; }
        Type HttpPostAttribute { get; }
        Type HttpDeleteAttribute { get; }
        Type IgnoreActionAttribute { get; }
        Type RouteAttribute { get; }

        Func<Object, string> GetTemplateFromRouteAttribute { get; }
        Func<Object, string> GetTemplateFromHttpMethodAttribute { get; }

        (Type type, string tsType)[] DefaultStaticMappings { get; }
    }

    public abstract class WebApiConfig : IWebApiConfig
    {
        public Type ControllerType { get; protected set; }
        public Type FromBodyAttribute { get; protected set; }
        public Type FromQueryAttribute { get;protected  set; }
        public Type FromRouteAttribute { get; protected set; }
        public Type HttpMethodBaseAttribute { get; protected set; }
        public Type HttpGetAttribute { get;protected  set; }
        public Type HttpPutAttribute { get;protected  set; }
        public Type HttpPostAttribute { get; protected set; }
        public Type HttpDeleteAttribute { get;protected  set; }
        public Type IgnoreActionAttribute { get;protected  set; }
        public Type RouteAttribute { get;protected  set; }
        public Func<object, string> GetTemplateFromRouteAttribute { get; protected set; }
        public Func<object, string> GetTemplateFromHttpMethodAttribute { get;protected  set; }
        public (Type type, string tsType)[] DefaultStaticMappings { get; protected set; }
    }
}
