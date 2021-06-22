namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Square
    {
        public int Side { get; set; }
        public const bool IsRound = false;
        public const int Sides = 4;
        public const string Kind = nameof(Square);
    }

    public class Rectangle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public const int Sides = 4;
        public const bool IsRound = false;
        public const string Kind = nameof(Rectangle);
    }
}
