using BlurkCompare;
using NUnit.Framework;
using RPS.myProjects.Infrastructure.Util;
using TeeSquare.Tests.Reflection.FakeDomain;

namespace TeeSquare.UnionTypes.Tests
{
    [TestFixture]
    public class UnionTypesTests
    {
        [Test]
        public void BasicUnionType()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareUnionTypes.Configure)
                .AddTypes(typeof(Shape))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
        [Test]
        public void GenericUnionType()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareUnionTypes.Configure)
                .AddTypes(typeof(Result<>))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
