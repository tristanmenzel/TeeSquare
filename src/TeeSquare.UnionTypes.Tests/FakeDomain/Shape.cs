using TeeSquare.UnionTypes;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    [UnionType(typeof(Circle), typeof(Square), typeof(Rectangle))]
    public abstract class Shape
    {
    }
}
