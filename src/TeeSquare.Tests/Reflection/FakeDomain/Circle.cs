using TeeSquare.Reflection;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Circle
    {
        public int Radius { get; set; }
        [TypeDiscriminator]
        public string Kind => nameof(Circle);
    }
}