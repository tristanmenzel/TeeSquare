using System.Reflection;
using BlurkCompare;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.Reflection;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenUsingRequestFactoryNameStrategy
    {
        public Assembly WebApiAssembly => typeof(ValuesController).Assembly;

        [Test]
        public void RoutesAreSplitAcrossAllocatedClassesWhichMatchTheRequestFactoryName()
        {
            var res = TeeSquareWebApi.GenerateForAssemblies(WebApiAssembly)
                .Configure(options =>
                {
                    options.TypeConverter = new TypeConverter((typeof(IFormFile), "File"));
                    options.FactoryNameStrategy = (controller, action, route) =>
                        $"{controller.Name.Replace("Controller", "")}RequestFactory";
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);

        }
    }
}
