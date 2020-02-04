using System.Reflection;

namespace TeeSquare.WebApi.Reflection
{
    public static class WebApiTypeExtension
    {
        public static bool IsAction(this MethodInfo action)
        {
            return !action.IsDefined(StaticConfig.IgnoreActionAttribute)
                   && !action.IsSpecialName
                   && !action.DeclaringType.Namespace.StartsWith("System")
                   && !action.DeclaringType.Namespace.StartsWith("Microsoft");
        }
    }
}
