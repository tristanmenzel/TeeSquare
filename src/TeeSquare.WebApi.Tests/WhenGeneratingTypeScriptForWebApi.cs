using System;
using System.Reflection;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenGeneratingTypeScriptForWebApi
    {
        public Assembly WebApiAssembly => typeof(ValuesController).Assembly;

        
        [Test]
        public void AllRoutesAndDtosAreOutput()
        {
            TeeSquareWebApi.GenerateForAssemblies(WebApiAssembly)
                .WriteToFile("test.ts");

        }
    }
}