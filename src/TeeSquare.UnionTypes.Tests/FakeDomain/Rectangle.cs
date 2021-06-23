using TeeSquare.UnionTypes;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Rectangle : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        [AsConst(nameof(Rectangle))]
        public string Kind => nameof(Rectangle);
    }
}
