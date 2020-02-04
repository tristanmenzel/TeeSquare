using System;

namespace TeeSquare.WebApi.Reflection
{
    public static class StaticConfig
    {
        public static Type ControllerType { get; set; }

        public static Type FromBodyAttribute { get; set; }
        public static Type FromQueryAttribute { get; set; }
        public static Type FromRouteAttribute { get; set; }

        public static Type HttpMethodBaseAttribute { get; set; }
        public static Type HttpGetAttribute { get; set; }
        public static Type HttpPutAttribute { get; set; }
        public static Type HttpPostAttribute { get; set; }
        public static Type HttpDeleteAttribute { get; set; }
        public static Type IgnoreActionAttribute { get; set; }
        public static Type RouteAttribute { get; set; }

        public static Func<Object, string> GetTemplateFromRouteAttribute { get; set; }
        public static Func<Object, string> GetTemplateFromHttpMethodAttribute { get; set; }

        public static (Type type, string tsType)[] DefaultStaticMappings { get; set; }
    }
}
