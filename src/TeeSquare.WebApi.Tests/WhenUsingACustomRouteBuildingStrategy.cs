using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenUsingACustomRouteBuildingStrategy
    {
        [Test]
        public void TheRouteDeterminedByTheStrategyIsUsed()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(ReturnValueAttributeController))
                .Configure(options =>
                {
                    var defaultStrategy = options.BuildRouteStrategy;
                    options.BuildRouteStrategy = (controller, action, defaultRoute) =>
                        $"prefix/{defaultStrategy(controller, action, defaultRoute)}";
                    options.NameRouteStrategy = (controller, action, uri, method) =>
                        $"{controller.Name.Replace("Controller", "")}_{action.Name}";
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
