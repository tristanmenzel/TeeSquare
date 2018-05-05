using System;
using System.IO;
using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Writers;

namespace TeeSquare.Tests.CodeWriterTests
{
    [TestFixture]
    public class CodeWriterTests
    {
        [Test]
        public void ContentBetweenBracesIsIndented()
        {
            var res = WriteToMemory(w =>
            {
                w.OpenBrace();
                w.WriteLine("let i = 0;");
                w.WriteLine("return i;");
                w.CloseBrace();
            });
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }

        [Test]
        public void ManualIndentingIsMaintained()
        {
            Utilty.CreateFileIfNotExists();
            var res = WriteToMemory(w =>
            {
                w.WriteLine(".");
                w.Indent();
                w.WriteLine(".");
                w.Indent();
                w.WriteLine(".");
                w.Indent();
                w.WriteLine(".");
                w.Indent();
                w.WriteLine(".");
                w.Indent();
                w.WriteLine(".");
                w.Indent();
                w.WriteLine(".");
                w.Deindent();
                w.WriteLine(".");
                w.Deindent();
                w.WriteLine(".");
                w.Deindent();
                w.WriteLine(".");
                w.Deindent();
                w.WriteLine(".");
                w.Deindent();
                w.WriteLine(".");
            });
            Blurk.CompareImplicitFile()
                .To(res)
                .AssertAreTheSame(Assert.Fail);
            
        }

        

        private string WriteToMemory(Action<ICodeWriter> writeAction)
        {
            using (var ms = new MemoryStream())
            using (var w = new CodeWriter(ms, "  "))
            using (var r = new StreamReader(ms))
            {
                writeAction(w);
                w.Flush();
                ms.Position = 0;
                return r.ReadToEnd();
            }
        }
    }
}