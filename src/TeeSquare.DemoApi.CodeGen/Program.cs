using System;
using System.Linq;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.WebApi;

namespace TeeSquare.DemoApi.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var outputPath = args
                .SkipWhile(a => a != "-o")
                .Skip(1)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(outputPath))
            {
                throw new ArgumentException("Must provide value for output file '-o <output file path>'");
            }
            Console.WriteLine(outputPath);
            TeeSquareWebApi.GenerateForAssemblies(typeof(OtherController).Assembly)
                .Configure(options =>
                {
                    options.ReflectMethods = type =>
                        type.IsInterface && (type.Namespace?.StartsWith("TeeSquare") ?? false);})
                .WriteToFile(outputPath);

        }
    }
}
