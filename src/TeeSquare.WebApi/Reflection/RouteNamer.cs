using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using TeeSquare.Reflection;

namespace TeeSquare.WebApi.Reflection
{
    public class RouteNamer : Namer
    {
        public override bool HasStaticMapping(Type type)
        {
            if (type == typeof(IActionResult))
                return true;
            return base.HasStaticMapping(type);
        }

        public override bool TryGetStaticMapping(Type type, out string name)
        {
            if (type == typeof(IActionResult))
            {
                name = "any";
                return true;
            }

            return base.TryGetStaticMapping(type, out name);
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