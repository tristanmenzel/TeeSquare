using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenControllerFilterIsSpecified
    {
        [Test]
        public void FilterControllersAreNotEmitted()
        {
            var res = TeeSquareWebApi.GenerateForAssemblies(typeof(DefaultRouteController).Assembly)
                .WithControllerFilter(c => c != typeof(FormValueController))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
