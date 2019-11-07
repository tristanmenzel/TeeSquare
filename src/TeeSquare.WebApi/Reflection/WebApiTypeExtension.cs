using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.WebApi.Reflection
{
    public static class WebApiTypeExtension
    {
        public static bool IsAction(this MethodInfo action)
        {
            return !action.IsDefined(typeof(NonActionAttribute))
                   && !action.IsSpecialName
                   && !action.DeclaringType.Namespace.StartsWith("System")
                   && !action.DeclaringType.Namespace.StartsWith("Microsoft");
        }
    }
}
