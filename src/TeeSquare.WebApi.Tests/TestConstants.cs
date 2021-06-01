using System;
using System.Reflection;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    public static class TestConstants
    {
        public static Assembly WebApiAssembly => typeof(ValuesController).Assembly;

        public static Type[] SimpleControllers =
        {
            typeof(DefaultRouteController),
            typeof(FormValueController),
            typeof(ImplicitParametersController),
            typeof(OtherController),
            typeof(ReturnValueAttributeController),
            typeof(RouteConstraintsController),
            typeof(RouteOnActionController),
            typeof(TestController),
            typeof(ValuesController),
        };
    }
}
