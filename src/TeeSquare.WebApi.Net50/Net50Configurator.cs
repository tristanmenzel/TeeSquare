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
            public Net50Configuration():base(
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
