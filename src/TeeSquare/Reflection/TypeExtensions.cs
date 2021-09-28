using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TeeSquare.Reflection
{
    public static class TypeExtensions
    {
        public static Type UnwrapNullable(this Type type)
        {
            return type.IsNullable(out var underlyingType) ? underlyingType : type;
        }

        public static bool IsNullable(this Type type, [NotNullWhen(true)] out Type? underlyingType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                underlyingType = type.GetGenericArguments()[0];
                return true;
            }

            underlyingType = null;
            return false;
        }

        public static bool IsNullable(this Type type) => type.IsNullable(out _);

        public static bool IsCollection(this Type type)
        {
            return type.IsCollection(out _);
        }

        public static bool IsCollection(this Type type, [NotNullWhen(true)] out Type? itemType)
        {
            if (type == typeof(string))
            {
                itemType = null;
                return false;
            }
            if (type.IsArray)
            {
                itemType = type.GetElementType()!;
                return true;
            }

            if (type.IsGenericType && typeof(IEnumerable<>)
                    .MakeGenericType(type.GetGenericArguments().First()).IsAssignableFrom(type))
            {
                itemType = type.GetGenericArguments().First();
                return true;
            }

            itemType = null;
            return false;
        }

        public static bool IsTask(this Type type, [NotNullWhen(true)] out Type? resultType)
        {
            if (type.IsGenericType && typeof(Task<>)
                    .MakeGenericType(type.GetGenericArguments().First()).IsAssignableFrom(type))
            {
                resultType = type.GetGenericArguments().First();
                return true;
            }

            if (type == typeof(Task))
            {
                resultType = typeof(void);
                return true;
            }

            resultType = null;
            return false;
        }

        public static bool IsDictionary(this Type type, [NotNullWhen(true)] out Type[]? genericTypeParams)
        {
            if (type.IsGenericType && type.GetGenericArguments().Length == 2 && typeof(IDictionary<,>)
                    .MakeGenericType(
                        type.GetGenericArguments().First(),
                        type.GetGenericArguments().Skip(1).First()).IsAssignableFrom(type))
            {
                genericTypeParams = type.GetGenericArguments();
                return true;
            }

            genericTypeParams = null;
            return false;
        }


    }
}
