using System;
using System.IO;
using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;

namespace TeeSquare.Tests.TypeScriptWriterTest
{
    [TestFixture]
    public class TypeScriptWriterTests
    {
        [Test]
        public void WriteInterface()
        {
            var res = TeeSquareFluent.StandardWriter()
                .Add(w =>
                {
                    w.WriteInterface("FunFunInterface", new TypeReference("TOne"), new TypeReference("TTwo"))
                        .Configure(i =>
                        {
                            i.AddMethod("TestMethod")
                                .WithReturnType(new TypeReference("string"))
                                .WithParams(p =>
                                {
                                    p.Param("a", new TypeReference("number"));
                                    p.Param("b", new TypeReference("Enumerable", "number"));
                                });
                            i.AddProperty("ValueOfTwo", new TypeReference("TTwo"));
                            i.AddProperty("MaybeItsOne", new TypeReference("Maybe", "TOne"));
                        });
                })
                .WriteToString();
            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void WriteClass()
        {
            var res = TeeSquareFluent.StandardWriter()
                .Add(w =>
                {
                    w.WriteClass("AppleService")
                        .Configure(c =>
                        {
                            c.AddProperty("banana", new TypeReference("string"));
                            c.AddMethod("getApplePie")
                                .WithReturnType(new TypeReference("string"))
                                .WithParams(p =>
                                {
                                    p.Param("numApples", new TypeReference("number"));
                                    p.Param("typeOfApple", new TypeReference("string"));
                                })
                                .WithBody(x => { x.WriteLine("return \"No apples here\";"); });
                            c.AddMethod("haveFun")
                                .WithParams(p => { p.Param("amountOfFun", new TypeReference("number")); })
                                .Static()
                                .WithBody(x => x.WriteLine("console.log(\"Having so much fun\", amountOfFun);"));
                        });
                }).WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void WriteEnum()
        {
            var res = TeeSquareFluent.StandardWriter()
                .Configure(options =>
                {
                    options.EnumWriterFactory = new EnumWriterFactory()
                        .IncludeDescriptions();
                })
                .Add(w =>
                {
                    w.WriteEnum("Fruits")
                        .Configure(e =>
                        {
                            e.AddValue("Apple", 0);
                            e.AddValue("Banana", 1);
                            e.AddValue("Cantelope", 2);
                        });

                    w.WriteEnum("Things", EnumValueType.String)
                        .Configure(e =>
                        {
                            e.AddValue("ThingOne", "ValueOne", "Description of thing one");
                            e.AddValue("ThingTwo", "ValueTwo", "Description of thing two");
                            e.AddValue("ThingThree", "ValueThree", "Description of thing three");
                        });
                }).WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void WriteFunction()
        {
            var res = TeeSquareFluent.StandardWriter()
                .Add(w =>
                {
                    w.WriteFunction("HelloWorld", "T1 extends number")
                        .WithReturnType(new TypeReference("number"))
                        .WithBody(body =>
                        {
                            body.WriteLine("console.log('Hey');");
                            body.WriteLine("return 2 * a;");
                        })
                        .WithParams(p => { p.Param("a", new TypeReference("T1")); });
                    w.WriteFunction("HelloWorldArrows")
                        .WithReturnType(new TypeReference("Array", "number"))
                        .WithBody(body =>
                        {
                            body.WriteLine("console.log('Hey');");
                            body.WriteLine("return [2, a];");
                        })
                        .WithParams(p => { p.Param("a", new TypeReference("number")); });
                    w.WriteFunction("LogSomething")
                        .WithBody(body => { body.WriteLine("console.log(a, b, c);"); })
                        .WithParams(p =>
                        {
                            p.Param("a", new TypeReference("number"));
                            p.Param("b", new TypeReference("Array", "number"));
                            p.Param("c", new TypeReference("string"));
                        });
                }).WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
