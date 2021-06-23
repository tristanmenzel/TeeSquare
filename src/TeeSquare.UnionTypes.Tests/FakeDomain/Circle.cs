
using TeeSquare.UnionTypes;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Circle : Shape
    {
        public int Radius { get; set; }
        [AsConst(nameof(Circle))]
        public string Kind => nameof(Circle);
    }
}
