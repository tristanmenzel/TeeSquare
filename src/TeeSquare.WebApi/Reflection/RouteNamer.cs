using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;

namespace TeeSquare.WebApi.Reflection
{
    public class RouteNamer : Namer
    {
        public override ITypeReference Type(Type type, bool optional = false)
        {
            if (type == typeof(IActionResult))
            {
                return new TypeReference("unknown"){ExistingType = true};
            }
            return base.Type(type, optional);
        }

        public virtual string RouteName(Type controller, MethodInfo action, string route)
        {
            var parts = route.Split('/')
                .Select(part =>
                {
                    if (part.StartsWith("{"))
                    {
                        return $"By{ToCase(part.Substring(1, part.Length - 2), NameConvention.PascalCase)}";
                    }

                    return ToCase(part, NameConvention.PascalCase);
                });
            return string.Join("", parts).Replace("[controller]", ControllerName(controller));
        }

        public string ControllerName(Type controller)
        {
            return controller.Name.Replace("Controller", "");
        }
    }
}
