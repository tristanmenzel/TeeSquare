using System;
using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Configuration;
using TeeSquare.Reflection;
using TeeSquare.Tests.Reflection.Enums;
using TeeSquare.Tests.Reflection.FakeDomain;
using TeeSquare.Tests.Reflection.NullableReferenceTypes;
using TeeSquare.Tests.Reflection.OpenGenerics;
using TeeSquare.Tests.Reflection.TreeDomain;
using TeeSquare.TypeMetadata;
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
        public void EnumAlternativeSizes()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(ByteEnum), typeof(LongEnum), typeof(UnsignedLongEnum))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void EnumAsStrings()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options => { options.EnumValueTypeStrategy = EnumValueTypeStrategies.AllString; })
                .AddTypes(typeof(Title))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void MixedEnums()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options =>
                {
                    options.EnumValueTypeStrategy = type =>
                        type == typeof(ByteEnum) ? EnumValueType.Number : EnumValueType.String;
                })
                .AddTypes(typeof(Title), typeof(ByteEnum))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void EnumDescriptions()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(DescribedEnum))
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
        public void ImportDependencies()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddImportedTypes(("./ReflectiveWriterTests.SmallTree", new[] {typeof(Title)}))
                .AddTypes(typeof(Name))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void UnusedImportsAreOmitted()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddImportedTypes(("./ReflectiveWriterTests.SmallTree",
                    new[] {typeof(Title), typeof(Book), typeof(Library)}))
                .AddTypes(typeof(Name))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void RenamedImports()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options =>
                {
                    options.TypeConverter.TypeName = options.TypeConverter.TypeName.ExtendStrategy(original =>
                        type => type == typeof(Book) ? $"{original(type)}Renamed" : original(type));
                    options.ImportTypeConverter = new TypeConverter();
                })
                .AddImportedTypes(("./ReflectiveWriterTests.EntireTree", new[]
                {
                    typeof(Location),
                    typeof(Book),
                }))
                .AddTypes(typeof(Library))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void OriginalCase()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options => { options.TypeConverter.NamingConventions.Properties = NameConvention.Original; })
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
        public void NullablePropertiesAsUndefinedUnion()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options =>
                    options.InterfaceWriterFactory =
                        new InterfaceWriterFactory(OptionalFieldRendering.WithUndefinedUnion))
                .AddTypes(typeof(Book))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void NullablePropertiesAsNullUnion()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options =>
                    options.InterfaceWriterFactory =
                        new InterfaceWriterFactory(OptionalFieldRendering.WithNullUnion))
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
                .Configure(options =>
                {
                    options.ComplexTypeStrategy = (writer, typeInfo, type) => writer.WriteClass(typeInfo);
                })
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
        public void OpenGenerics()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Result<>), typeof(Result<int>))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void ReflectMethods()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options => { options.ReflectMethods = t => t.IsInterface; })
                .AddTypes(typeof(ISampleApi))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void ReflectMethod()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .Configure(options =>
                {
                    options.ReflectMethods = t => t.IsInterface;
                    options.ReflectMethod = (t, mi) => mi.Name == "GetBook";
                })
                .AddTypes(typeof(ISampleApi))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void ReflectFields()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(Audit))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void ReflectRecursiveTree()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(TreeNode))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void NullableReferenceTypes()
        {
            var res = TeeSquareFluent.ReflectiveWriter()
                .AddTypes(typeof(TypeWithNullableReference))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
