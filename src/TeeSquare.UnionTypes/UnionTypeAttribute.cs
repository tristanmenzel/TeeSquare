using System;

namespace TeeSquare.UnionTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class UnionTypeAttribute : Attribute
    {
        public Type[] UnionTypes { get; }

        public UnionTypeAttribute(params Type[] unionTypes)
        {
            UnionTypes = unionTypes;
        }
    }
}
