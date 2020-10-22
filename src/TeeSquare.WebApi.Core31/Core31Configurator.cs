using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Core31
{
    public static class Core31Configurator
    {
        public static void Configure()
        {
            StaticConfig.Configure(new Core31Configuration());
        }

        class Core31Configuration : WebApiConfig
        {
            public Core31Configuration()
            {
                ControllerType = typeof(ControllerBase);

                FromFormAttribute = typeof(FromFormAttribute);
                FromBodyAttribute = typeof(FromBodyAttribute);
                FromQueryAttribute = typeof(FromQueryAttribute);
                FromRouteAttribute = typeof(FromRouteAttribute);

                HttpMethodBaseAttribute = typeof(HttpMethodAttribute);
                HttpGetAttribute = typeof(HttpGetAttribute);
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
