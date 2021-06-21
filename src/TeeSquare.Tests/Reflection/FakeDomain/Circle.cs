
using System.Security.Cryptography;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Circle : Shape
    {
        public int Radius { get; set; }
        public const int Sides = 0;
        public const bool IsRound = true;
        public const string Kind = nameof(Circle);
    }
}
