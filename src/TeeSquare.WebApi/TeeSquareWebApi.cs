using System;
using System.Collections.Generic;
using System.Reflection;
using TeeSquare.Reflection;

namespace TeeSquare.WebApi
{
    public class TeeSquareWebApi
    {
        public static WebApiFluent GenerateForAssemblies(params Assembly[] assemblies)
        {
            return new WebApiFluent(assemblies);
        }

        public static WebApiFluent GenerateForControllers(params Type[] controllers)
        {
            return new WebApiFluent(controllers);
        }
    }
}
