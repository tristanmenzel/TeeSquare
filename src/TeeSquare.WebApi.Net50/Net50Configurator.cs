using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Net50
{
    public static class Net50Configurator
    {
        static Net50Configurator()
        {
            StaticConfig.Configure(new Net50Configuration());
        }
        public static void Configure()
        {
            StaticConfig.Configure(new Net50Configuration());
        }

        class Net50Configuration : WebApiConfig
        {
            public Net50Configuration()
            {
                ControllerType = typeof(ControllerBase);

                FromFormAttribute = typeof(FromFormAttribute);
                FromBodyAttribute = typeof(FromBodyAttribute);
                FromQueryAttribute = typeof(FromQueryAttribute);
                FromRouteAttribute = typeof(FromRouteAttribute);

                HttpMethodBaseAttribute = typeof(HttpMethodAttribute);
                HttpGetAttribute = typeof(HttpGetAttribute);
                HttpPatchAttribute = typeof(HttpPatchAttribute);
                HttpOptionsAttribute = typeof(HttpOptionsAttribute);
                HttpPutAttribute = typeof(HttpPutAttribute);
                HttpPostAttribute = typeof(HttpPostAttribute);
                HttpDeleteAttribute = typeof(HttpDeleteAttribute);
                IgnoreActionAttribute = typeof(NonActionAttribute);
                RouteAttribute = typeof(RouteAttribute);

                GetTemplateFromRouteAttribute = obj =>
                {
                    if (obj is RouteAttribute routeAttribute)
                        return routeAttribute.Template;
                    return null;
                };
                GetTemplateFromHttpMethodAttribute = obj =>
                {
                    if (obj is HttpMethodAttribute httpMethodAttribute)
                        return httpMethodAttribute.Template;
                    return null;
                };
                DefaultStaticMappings = new[] {(typeof(IActionResult), "unknown")};
            }
        }
    }
}
