using BlurkCompare;
using NUnit.Framework;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenNoControllersAreReferenced
    {
        [Test]
        public void OnlySharedTypesAreEmitted()
        {
            var res = TeeSquareWebApi.GenerateForControllers()
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
