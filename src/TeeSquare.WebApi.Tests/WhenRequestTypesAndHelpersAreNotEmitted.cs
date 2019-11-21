using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

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
                    options.EmitRequestTypesAndHelpers = false;
                    options.Imports = new[] {
                        (types: new[] {"GetRequest", "PutRequest", "PostRequest", "DeleteRequest"},
                           path:  "./WhenNoControllersAreReferenced.OnlySharedTypesAreEmitted")
                    };
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
