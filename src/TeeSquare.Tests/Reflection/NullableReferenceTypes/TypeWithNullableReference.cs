#nullable enable
namespace TeeSquare.Tests.Reflection.NullableReferenceTypes
{
    public class TypeWithNullableReference
    {
        public Banana MandatoryBanana { get; set; } = new Banana();
        public Banana? OptionalBanana { get; set; } = null;
    }

    public class Banana
    {
        public int Size { get; set; }
    }
}
