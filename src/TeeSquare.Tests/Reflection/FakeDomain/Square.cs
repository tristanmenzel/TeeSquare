using TeeSquare.Reflection;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Square
    {
        public int Side { get; set; }
        [TypeDiscriminator]
        public string Kind => nameof(Square);
    }

    public class Rectangle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        [TypeDiscriminator("SpecialRectangle")]
        public string Kind => "SpecialRectangle";
    }
}