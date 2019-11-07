using System;
using System.ComponentModel.DataAnnotations.Schema;
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
            var res = WriteToMemory(w =>
            {
                w.WriteInterface("FunFunInterface", "TOne", "TTwo")
                    .Configure(i =>
                    {
                        i.AddMethod("TestMethod")
                            .WithReturnType("string")
                            .WithParams(p =>
                            {
                                p.Param("a", "number");
                                p.Param("b", "Enumerable", "number");
                            });
                        i.AddProperty("ValueOfTwo", "TTwo");
                        i.AddProperty("MaybeItsOne", "Maybe", "TOne");
                    });
            });
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void WriteClass()
        {
            var res = WriteToMemory(w =>
            {
                w.WriteClass("AppleService")
                    .Configure(c =>
                    {
                        c.AddProperty("banana", "string");
                        c.AddMethod("getApplePie")
                            .WithReturnType("string")
                            .WithParams(p =>
                            {
                                p.Param("numApples", "number");
                                p.Param("typeOfApple", "string");
                            })
                            .WithBody(x => { x.WriteLine("return \"No apples here\";"); });
                        c.AddMethod("haveFun")
                            .WithParams(p => { p.Param("amountOfFun", "number"); })
                            .Static()
                            .WithBody(x => x.WriteLine("console.log(\"Having so much fun\", amountOfFun);"));
                    });
            });

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void WriteEnum()
        {
            var options = new WriterOptions();
            options.EnumWriterFactory = new EnumWriterFactory()
                .IncludeDescriptions();
            var res = WriteToMemory(w =>
            {
                w.WriteEnum("Fruits")
                    .Configure(e =>
                    {
                        e.AddValue("Apple", 0);
                        e.AddValue("Banana", 1);
                        e.AddValue("Cantelope", 2);
                    });

                w.WriteEnum("Things")
                    .Configure(e =>
                    {
                        e.AddValue("ThingOne", "ValueOne", "Description of thing one");
                        e.AddValue("ThingTwo", "ValueTwo", "Description of thing two");
                        e.AddValue("ThingThree", "ValueThree", "Description of thing three");
                    });
            }, options);

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void WriteFunction()
        {
            var res = WriteToMemory(w =>
            {
                w.WriteFunction("HelloWorld")
                    .WithGenericTypeParams("T1 extends number")
                    .WithReturnType("number")
                    .WithBody(body =>
                    {
                        body.WriteLine("console.log('Hey');");
                        body.WriteLine("return 2 * a;");
                    })
                    .WithParams(p => { p.Param("a", "T1"); });
                w.WriteFunction("HelloWorldArrows")
                    .WithReturnType("Array", "number")
                    .WithBody(body =>
                    {
                        body.WriteLine("console.log('Hey');");
                        body.WriteLine("return [2, a];");
                    })
                    .WithParams(p => { p.Param("a", "number"); });
                w.WriteFunction("LogSomething")
                    .WithBody(body => { body.WriteLine("console.log(a, b, c);"); })
                    .WithParams(p =>
                    {
                        p.Param("a", "number");
                        p.Param("b", "Array", "number");
                        p.Param("c", "string");
                    });
            });

            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        public static string WriteToMemory(Action<TypeScriptWriter> writeAction,
            WriterOptions options= null)
        {
            options ??= new WriterOptions();
            using (var ms = new MemoryStream())
            using (var w = new TypeScriptWriter(ms,
                options.InterfaceWriterFactory,
                options.ClassWriterFactory,
                options.EnumWriterFactory,
                options.FunctionWriterFactory,
                options.IndentChars))
            {
                writeAction(w);
                w.Flush();
                ms.Position = 0;
                var res = ms.ToArray();
                using (var reader = new StreamReader(new MemoryStream(res)))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
