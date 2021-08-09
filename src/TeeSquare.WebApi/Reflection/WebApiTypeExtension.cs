using System.Reflection;

namespace TeeSquare.WebApi.Reflection
{
    public static class WebApiTypeExtension
    {
        public static bool IsAction(this MethodInfo action)
        {
            return !action.IsDefined(StaticConfig.Instance.IgnoreActionAttribute)
                   && !action.IsSpecialName
                   && !(action.DeclaringType?.Namespace?.StartsWith("System") ?? false)
                   && !(action.DeclaringType?.Namespace?.StartsWith("Microsoft") ?? false);
        }
    }
}
