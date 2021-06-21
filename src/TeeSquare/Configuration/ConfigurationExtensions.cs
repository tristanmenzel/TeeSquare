using System;

namespace TeeSquare.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T ExtendStrategy<T>(this T original, Func<T, T> fn) where T : Delegate
        {
            return fn(original);
        }
    }
}
