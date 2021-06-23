
using System;
using TeeSquare.Reflection;

namespace TeeSquare.UnionTypes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AsConstAttribute : Attribute
    {
        public readonly string ConstType;
        public AsConstAttribute(object value)
        {
            ConstType = TypeExtensions.ConvertCsLiteralToJsLiteral(value);
        }

    }
}
