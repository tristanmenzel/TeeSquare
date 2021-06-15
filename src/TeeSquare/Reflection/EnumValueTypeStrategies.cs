using System;
using TeeSquare.TypeMetadata;

namespace TeeSquare.Reflection
{
    public static class EnumValueTypeStrategies
    {
        public static EnumValueType AllNumber(Type type) => EnumValueType.Number;
        public static EnumValueType AllString(Type type) => EnumValueType.String;
    }
}
