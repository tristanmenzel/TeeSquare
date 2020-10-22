using System.Reflection;
using BlurkCompare;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.DemoApi.Dtos;
using TeeSquare.Reflection;

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
