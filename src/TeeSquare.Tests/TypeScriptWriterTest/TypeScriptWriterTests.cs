using System;
using System.IO;
using BlurkCompare;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using TeeSquare.Writers;

namespace TeeSquare.Tests.TypeScriptWriterTests
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
                    .With(i =>
                    {
                        i.Method("TestMethod", "string")
                            .WithParams(p =>
                            {
                                p.Param("a", "number");
                                p.Param("b", "Enumerable", "number");
                            });
                        i.Property("ValueOfTwo", "TTwo");
                        i.Property("MaybeitsOne", "Maybe", "TOne");
                    });
            });
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void WriteClass()
        {
            var res = WriteToMemory(w => { w.WriteClass("AppleService")
                .With(c =>
                {
                    c.Property("banana", "string");
                    c.Method("getApplePie", "string")
                        .WithParams(p =>
                        {

                            p.Param("numApples", "number");
                            p.Param("typeOfApple", "string");
                        })
                        .WithBody(x => { x.WriteLine("return \"No apples here\";"); });
                    c.Method("haveFun", "void")
                        .WithParams(p => { p.Param("amountOfFun", "number"); })
                        .Static()
                        .WithBody(x => x.WriteLine("console.log(\"Having so much fun\", amountOfFun);"));
                }); });
            
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }


        [Test]
        public void WriteEnum()
        {
            Utilty.CreateFileIfNotExists();

            var res = WriteToMemory(w =>
            {
                w.WriteEnum("Fruits")
                    .WithValues(e =>
                    {
                        e.Value("Apple", 0);
                        e.Value("Banana", 1);
                        e.Value("Cantelope", 2);
                    });

                w.WriteEnum("Things")
                    .WithValues(e =>
                    {
                        e.Value("ThingOne", "ValueOne", "Description of thing one");
                        e.Value("ThingTwo", "ValueTwo", "Description of thing two");
                        e.Value("ThingThree", "ValueThree", "Description of thing three");
                    });
            });
            
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);

        }


        private string WriteToMemory(Action<TypeScriptWriter> writeAction)
        {
            using (var ms = new MemoryStream())
            using (var w = new TypeScriptWriter(ms, "  "))
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