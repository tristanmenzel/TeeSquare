using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenGeneratingTypeScriptForWebApiController
    {
        [Test]
        public void OnlyTypesForThatControllerAreOutput()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(TestController))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
