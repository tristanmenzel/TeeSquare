using System.Linq;
using System.Reflection;
using BlurkCompare;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using TeeSquare.DemoApi.Controllers;

namespace TeeSquare.WebApi.Tests
{
    [TestFixture]
    public class WhenUsingACustomReturnTypeSniffingStrategy
    {
        [Test]
        public void TheTypeDeterminedByTheStrategyIsUsed()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(ReturnValueAttributeController))
                .Configure(options =>
                {
                    options.GetApiReturnTypeStrategy = (controller, action) =>
                        action.GetCustomAttributes<ProducesResponseTypeAttribute>()
                            .Select(a => a.Type)
                            .FirstOrDefault() ?? action.ReturnType;
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
