using System;
using System.Reflection;
using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Reflection;
using TeeSquare.Tests.Reflection.FakeDomain;
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
                    options.TypeConverter = new CustomTypeConverter();
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

        public class CustomTypeConverter : TypeConverter
        {
            public override string TypeName(Type type)
            {
                if (type == typeof(Book))
                {
                    return $"{base.TypeName(type)}Renamed";
                }

                return base.TypeName(type);
            }
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
                .Configure(options =>
                {
                    options.TypeConverter = new DiscriminatedUnionTypeConverter();
                })
                .AddTypes(typeof(Circle), typeof(Square), typeof(Rectangle))
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        class DiscriminatedUnionTypeConverter : TypeConverter
        {
            public override ITypeReference Convert(Type type, Type parentType = null, MemberInfo info = null)
            {
                if (info is PropertyInfo pi)
                {
                    if (DiscriminatedUnionsHelper.IsDiscriminator(parentType, pi))
                    {
                        var value = DiscriminatedUnionsHelper.GetDiscriminatorValue(parentType, pi);
                        return new TypeReference($"'{value}'") {ExistingType = true};
                    }
                }

                return base.Convert(type, parentType, info);
            }
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
                .To(res, true)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
