using TeeSquare.TypeMetadata;

namespace TeeSquare.UnionTypes.Tests.FakeDomain
{
    public class SupportedDiscriminatorTypes
    {
        [AsConst(0)]
        public int Integer => 0;
        [AsConst("Test")]
        public string String => "Test";
        [AsConst(false)]
        public bool Boolean => false;
        [AsConst(EnumValueType.Number)]
        public EnumValueType Enum => EnumValueType.Number;
    }
}
