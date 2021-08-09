using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Core31
{
    public static class Core31Configurator
    {
        static Core31Configurator()
        {
            StaticConfig.Configure(new Core31Configuration());
        }

        public static void Configure()
        {
            StaticConfig.Configure(new Core31Configuration());
        }

        class Core31Configuration : WebApiConfig
        {
            public Core31Configuration() : base(
                typeof(ControllerBase),
                typeof(FromFormAttribute),
                typeof(FromBodyAttribute),
                typeof(FromQueryAttribute),
                typeof(FromRouteAttribute),
                typeof(HttpMethodAttribute),
                typeof(HttpGetAttribute),
                typeof(HttpPutAttribute),
                typeof(HttpPostAttribute),
                typeof(HttpDeleteAttribute),
                typeof(HttpPatchAttribute),
                typeof(HttpOptionsAttribute),
                typeof(NonActionAttribute),
                typeof(RouteAttribute),
                obj =>
                {
                    if (obj is RouteAttribute routeAttribute)
                        return routeAttribute.Template;
                    return null;
                }, obj =>
                {
                    if (obj is HttpMethodAttribute httpMethodAttribute)
                        return httpMethodAttribute.Template;
                    return null;
                },
                new[] { (typeof(IActionResult), "unknown") })
            {
            }
        }
    }
}
