using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Tests.Reflection.FakeDomain;
using TeeSquare.Writers;

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

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void Enum()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Title))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
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
                    o.EnumWriterFactory = new EnumWriterFactory()
                        .IncludeDescriptionGetter(true)
                        .IncludeAllValuesConst(true)
                        .IncludeDescriptions(true);
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void SmallTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
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
                .AddTypes(typeof(Book))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void EntireTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Library))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void OutputClassStrategy()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options => { options.ComplexTypeStrategy = (writer, type) => writer.WriteClass(type); })
                .AddTypes(typeof(Library))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void Generics()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Member))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void DiscriminatorProperty()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Circle), typeof(Square), typeof(Rectangle))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
