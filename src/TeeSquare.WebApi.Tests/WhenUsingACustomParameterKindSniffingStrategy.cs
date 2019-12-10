using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenUsingACustomParameterKindSniffingStrategy
    {
        [Test]
        public void TheKindDeterminedByTheStrategyIsUsed()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(OtherController))
                .Configure(options =>
                {
                    options.GetParameterKindStrategy = (paramInfo, route, method) => ParameterKind.Route;
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void NullableBodyTypesAreHandledCorrectly()
        {

            var res = TeeSquareWebApi.GenerateForControllers(typeof(OtherController))
                .Configure(options =>
                {
                    options.GetParameterKindStrategy = (paramInfo, route, method) => ParameterKind.Body;
                    options.GetHttpMethodAndRequestFactoryStrategy =
                        (controller, action) => (RequestInfo.Post, HttpMethod.Post);
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
