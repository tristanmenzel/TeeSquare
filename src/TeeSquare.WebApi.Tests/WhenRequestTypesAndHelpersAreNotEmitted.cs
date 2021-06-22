using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.WebApi.Reflection;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenRequestTypesAndHelpersAreNotEmitted
    {
        [Test]
        public void TheyCanBeImportedFromElsewhere()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(TestController))
                .Configure(options =>
                {
                    options.RequestHelperTypeOption = RequestHelperTypeOptions.ImportTypes("./WhenNoControllersAreReferenced.OnlySharedTypesAreEmitted");
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
