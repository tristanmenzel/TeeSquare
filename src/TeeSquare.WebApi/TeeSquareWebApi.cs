using System;
using System.Reflection;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi
{
    public class TeeSquareWebApi
    {
        public static WebApiFluent GenerateForAssemblies(params Assembly[] assemblies)
        {
            AssertConfigurationIsPresent();
            return new WebApiFluent(assemblies);
        }

        public static WebApiFluent GenerateForControllers(params Type[] controllers)
        {
            AssertConfigurationIsPresent();
            return new WebApiFluent(controllers);
        }

        private static void AssertConfigurationIsPresent()
        {
            if (StaticConfig.Instance != null)
                return;

            throw new InvalidOperationException(@"TeeSquare WebApi must be configured for a specific platform version.

Please install and invoke the relevant configurator for your platform.

Eg. TeeSquare.WebApi.Core31.Core31Configurator.Configure() ");
        }
    }
}
