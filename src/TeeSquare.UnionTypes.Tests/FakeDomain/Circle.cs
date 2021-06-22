
using System.Security.Cryptography;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Circle : Shape
    {
        public int Radius { get; set; }
        public const string Kind = nameof(Circle);
    }
}
