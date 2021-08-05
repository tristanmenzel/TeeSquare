using BlurkCompare;
using NUnit.Framework;
using TeeSquare.Configuration;
using TeeSquare.DemoApi.Controllers;
using TeeSquare.TypeMetadata;

namespace TeeSquare.WebApi.Tests
{
    /// <summary>
    /// Sometimes complex dotnet types will be serialized as TS/JS primitives and as such should be treated as
    /// one by code generation. An example would be the DateTime struct which TeeSquare handles by default.
    /// Overriding the TreatAsPrimitive strategy lets us implement this behaviour for other types.
    /// </summary>
    [TestFixture]
    public class WhenTreatingComplexTypesAsPrimitives
    {
        [Test]
        public void TheyAreTreatedLikePrimitives()
        {
            var res = TeeSquareWebApi.GenerateForControllers(typeof(FauxPrimitiveController))
                .Configure(options =>
                {

                    options.TypeConverter.TreatAsPrimitive =
                        options.TypeConverter.TreatAsPrimitive.ExtendStrategy(
                            original => type =>
                                type == typeof(FauxPrimitiveController.FauxPrimitive) || original(type));
                    options.ComplexTypeStrategy = options.ComplexTypeStrategy.ExtendStrategy(original => (writer, info, type) =>
                    {
                        if (info.Name == nameof(FauxPrimitiveController.FauxPrimitive))
                        {
                            writer.WriteLine($"export type {info.Name} = string");
                        }
                        else
                        {
                            original(writer, info, type);
                        }
                    });
                })
                .WriteToString();

            Blurk.CompareImplicitFile("ts")
                .To(res)
                .AssertAreTheSame(Assert.Fail);
        }
    }
}
