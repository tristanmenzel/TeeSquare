using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Core22
{
    public static class Core22Configurator
    {
        public static void Configure()
        {
            StaticConfig.Configure(new Core22Configuration());
        }

        class Core22Configuration : WebApiConfig
        {
            public Core22Configuration()
            {
                ControllerType = typeof(ControllerBase);

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
