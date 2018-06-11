using System;
using System.Collections.Generic;
using System.Linq;

namespace TeeSquare.Reflection
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type, out Type underlyingType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                underlyingType = type.GetGenericArguments()[0];
                return true;
            }

            underlyingType = null;
            return false;
        }

        public static bool IsCollection(this Type type, out Type itemType)
        {
            if (type.IsArray)
            {
                itemType = type.GetElementType();
                return true;
            }

            if (type.IsGenericType && typeof(ICollection<>)
                    .MakeGenericType(type.GetGenericArguments().First()).IsAssignableFrom(type))
            {
                itemType = type.GetGenericArguments().First();
                return true;
            }

            itemType = null;
            return false;
        }
    }
}