using BlurkCompare;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using TeeSquare.Reflection;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenGeneratingTypeScriptForWebApi
    {
        [Test]
        public void AllRoutesAndDtosAreOutput()
        {
            var res = TeeSquareWebApi.GenerateForAssemblies(TestConstants.WebApiAssembly)
                .Configure(options =>
                {
                    options.TypeConverter = new TypeConverter((typeof(IFormFile), "File"));
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
