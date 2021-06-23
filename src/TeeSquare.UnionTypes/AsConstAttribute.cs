
using System;
using TeeSquare.Reflection;

namespace TeeSquare.UnionTypes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AsConstAttribute : Attribute
    {
        public readonly object ConstValue;
        public AsConstAttribute(object value)
        {
            ConstValue = value;
        }

    }
}
