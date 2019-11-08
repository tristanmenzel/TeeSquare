using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Tests.Reflection.FakeDomain;

namespace TeeSquare.Mobx.Tests
{
    [TestFixture]
    public class MobxModelTests
    {
        [Test]
        public void LeafClass()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareMobx.ConfigureMobxWriter)
                .AddTypes(typeof(Location))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void SmallTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareMobx.ConfigureMobxWriter)
                .AddTypes(typeof(Name))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void NullableProperties()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareMobx.ConfigureMobxWriter)
                .AddTypes(typeof(Book))
                .WriteToString();

            TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareMobx.ConfigureMobxWriter)
                .AddTypes(typeof(Book))
                .WriteToFile(@"C:\Source\Tristan\TeeSquare\src\TeeSquare.Mobx.Tests\Test.ts");
            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void EntireTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(TeeSquareMobx.ConfigureMobxWriter)
                .AddTypes(typeof(Library))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
