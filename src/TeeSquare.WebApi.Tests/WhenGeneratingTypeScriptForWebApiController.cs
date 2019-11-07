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
        {TeeSquareWebApi.GenerateForControllers(typeof(TestController))
                .WriteToFile("test2.ts");
            var res = TeeSquareWebApi.GenerateForControllers(typeof(TestController))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
