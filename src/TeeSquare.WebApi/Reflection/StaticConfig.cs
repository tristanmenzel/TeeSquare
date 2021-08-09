using System;
using System.Configuration;

namespace TeeSquare.WebApi.Reflection
{
    public static class StaticConfig
    {
        private static IWebApiConfig? _instance;

        public static void Configure(IWebApiConfig configuration)
        {
            _instance = configuration;
        }

        public static IWebApiConfig Instance => _instance ?? throw new Exception(
            "WebApi reflector has not been configured. Please include a TeeSquare.WebApi.{Platform} library or provide a manual configuration.");
    }

    public interface IWebApiConfig
    {
        Type ControllerType { get; }
        Type FromFormAttribute { get; }
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
        Type HttpPatchAttribute { get; }
        Type HttpOptionsAttribute { get; }
    }

    public abstract class WebApiConfig : IWebApiConfig
    {
        protected WebApiConfig(
            Type controllerType,
            Type fromFormAttribute,
            Type fromBodyAttribute,
            Type fromQueryAttribute,
            Type fromRouteAttribute,
            Type httpMethodBaseAttribute,
            Type httpGetAttribute,
            Type httpPutAttribute,
            Type httpPostAttribute,
            Type httpDeleteAttribute,
            Type httpPatchAttribute,
            Type httpOptionsAttribute,
            Type ignoreActionAttribute,
            Type routeAttribute,
            Func<object, string> getTemplateFromRouteAttribute,
            Func<object, string> getTemplateFromHttpMethodAttribute,
            (Type type, string tsType)[] defaultStaticMappings)
        {
            ControllerType = controllerType;
            FromFormAttribute = fromFormAttribute;
            FromBodyAttribute = fromBodyAttribute;
            FromQueryAttribute = fromQueryAttribute;
            FromRouteAttribute = fromRouteAttribute;
            HttpMethodBaseAttribute = httpMethodBaseAttribute;
            HttpGetAttribute = httpGetAttribute;
            HttpPutAttribute = httpPutAttribute;
            HttpPostAttribute = httpPostAttribute;
            HttpDeleteAttribute = httpDeleteAttribute;
            IgnoreActionAttribute = ignoreActionAttribute;
            RouteAttribute = routeAttribute;
            GetTemplateFromRouteAttribute = getTemplateFromRouteAttribute;
            GetTemplateFromHttpMethodAttribute = getTemplateFromHttpMethodAttribute;
            DefaultStaticMappings = defaultStaticMappings;
            HttpPatchAttribute = httpPatchAttribute;
            HttpOptionsAttribute = httpOptionsAttribute;
        }

        public Type ControllerType { get;  }
        public Type FromFormAttribute { get;  }
        public Type FromBodyAttribute { get;  }
        public Type FromQueryAttribute { get;  }
        public Type FromRouteAttribute { get;  }
        public Type HttpMethodBaseAttribute { get;  }
        public Type HttpGetAttribute { get;  }
        public Type HttpPutAttribute { get;  }
        public Type HttpPostAttribute { get;  }
        public Type HttpDeleteAttribute { get;  }
        public Type IgnoreActionAttribute { get;  }
        public Type RouteAttribute { get;  }
        public Func<object, string> GetTemplateFromRouteAttribute { get;  }
        public Func<object, string> GetTemplateFromHttpMethodAttribute { get;  }
        public (Type type, string tsType)[] DefaultStaticMappings { get;  }
        public Type HttpPatchAttribute { get;  }
        public Type HttpOptionsAttribute { get;  }
    }
}
