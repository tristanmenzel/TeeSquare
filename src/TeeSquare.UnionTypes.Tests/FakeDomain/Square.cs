using TeeSquare.UnionTypes;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Square : Shape
    {
        public int Side { get; set; }
        [AsConst(nameof(Square))]
        public string Kind => nameof(Square);
    }
}
