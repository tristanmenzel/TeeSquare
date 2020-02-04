using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Core31
{
    public static class Core31Configurator
    {
        private static bool _configured = false;

        public static void Configure()
        {
            if (_configured) return;
            _configured = true;
            StaticConfig.ControllerType = typeof(ControllerBase);

            StaticConfig.FromBodyAttribute = typeof(FromBodyAttribute);
            StaticConfig.FromQueryAttribute = typeof(FromQueryAttribute);
            StaticConfig.FromRouteAttribute = typeof(FromRouteAttribute);

            StaticConfig.HttpMethodBaseAttribute = typeof(HttpMethodAttribute);
            StaticConfig.HttpGetAttribute = typeof(HttpGetAttribute);
            StaticConfig.HttpPutAttribute = typeof(HttpPutAttribute);
            StaticConfig.HttpPostAttribute = typeof(HttpPostAttribute);
            StaticConfig.HttpDeleteAttribute = typeof(HttpDeleteAttribute);
            StaticConfig.IgnoreActionAttribute = typeof(NonActionAttribute);
            StaticConfig.RouteAttribute = typeof(RouteAttribute);

            StaticConfig.GetTemplateFromRouteAttribute = obj =>
            {
                if (obj is RouteAttribute routeAttribute)
                    return routeAttribute.Template;
                return null;
            };
            StaticConfig.GetTemplateFromHttpMethodAttribute = obj =>
            {
                if (obj is HttpMethodAttribute httpMethodAttribute)
                    return httpMethodAttribute.Template;
                return null;
            };
            StaticConfig.DefaultStaticMappings = new[] {(typeof(IActionResult), "unknown")};
        }
    }
}
