using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenGeneratingTypeScriptForWebApiController
    {
        [Test]
        public void OnlyTypesForThatControllerAreOutput()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(RouteOnActionController))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
