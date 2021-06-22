using BlurkCompare;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using TeeSquare.DemoApi.Dtos;
using TeeSquare.Reflection;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenIncludingTypesNotExplicitlyUsedInTheApi
    {


        [Test]
        public void TheyAreIncludedInTheOutput()
        {
            var res = TeeSquareWebApi.GenerateForControllers(TestConstants.SimpleControllers)
                .Configure(options =>
                {
                    options.TypeConverter = new TypeConverter((typeof(IFormFile), "File"));
                })
                .AddTypes(typeof(NotUsedInApiTestDto))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
