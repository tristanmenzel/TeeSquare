using System;
using System.Linq;
using System.Reflection;
using TeeSquare.Reflection;
using TeeSquare.Util;

namespace TeeSquare.WebApi.Reflection
{
    public class RouteNamer
    {


        public virtual string RouteName(Type controller, MethodInfo action, string route, HttpMethod method)
        {
            var parts = route.Split('/')
                .Select(part =>
                {
                    if (part.StartsWith("{"))
                    {
                        return $"By{CaseHelper.ToCase(part.Substring(1, part.Length - 2), NameConvention.PascalCase)}";
                    }

                    return CaseHelper.ToCase(part, NameConvention.PascalCase);
                });
            return method.GetName() + string.Join("", parts).Replace("[controller]", ControllerName(controller));
        }

        public string ControllerName(Type controller)
        {
            return controller.Name.Replace("Controller", "");
        }
    }
}
