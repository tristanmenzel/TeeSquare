using System.Linq;
using System.Reflection;
using BlurkCompare;
using Microsoft.AspNetCore.Mvc;
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
                    options.BuildRouteStrategy = (controller, action) =>
                        $"prefix/{defaultStrategy(controller, action)}";
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
