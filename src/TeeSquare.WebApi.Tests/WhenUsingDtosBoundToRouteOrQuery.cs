using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenUsingDtosBoundToRouteOrQuery
    {
        [Test]
        public void TheRequestFactoryDestructuresTheDtoIntoTheRouteOrQuery()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(DtoFromRouteOrQueryController))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
