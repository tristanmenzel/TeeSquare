namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Square : Shape
    {
        public int Side { get; set; }
        public const string Kind = nameof(Square);
    }

    public class Rectangle : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public const string Kind = nameof(Rectangle);
    }
}
