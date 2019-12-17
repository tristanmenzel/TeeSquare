using System.Reflection;
using BlurkCompare;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.DemoApi.Dtos;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenIncludingTypesNotExplicitlyUsedInTheApi
    {
        public Assembly WebApiAssembly => typeof(ValuesController).Assembly;


        [Test]
        public void TheyAreIncludedInTheOutput()
        {
            var res = TeeSquareWebApi.GenerateForAssemblies(WebApiAssembly)
                .AddTypes(typeof(NotUsedInApiTestDto))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
