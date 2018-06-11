using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Reflection;
using TeeSquare.Tests.Reflection.FakeDomain;
using TeeSquare.Tests.TypeScriptWriterTest;

namespace TeeSquare.Tests.Reflection
{
    [TestFixture]
    public class ReflectiveWriterTests
    {
        [Test]
        public void LeafClass()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Location))
                .WriteToString();

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void Enum()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Title))
                .WriteToString();

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void EnumDescriptions()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Audience))
                .Configure(o =>
                {
                    o.WriteEnumDescriptionGetters = true;
                    o.WriteEnumDescriptions = true;
                })
                .WriteToString();

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void SmallTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Name))
                .WriteToString();

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void NullableProperties()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Book))
                .WriteToString();

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void EntireTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Library))
                .WriteToString();

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void Generics()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Member))
                .WriteToString();
           
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}