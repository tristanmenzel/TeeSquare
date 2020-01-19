using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeeSquare.TypeMetadata;
using MethodInfo = System.Reflection.MethodInfo;
using PropertyInfo = System.Reflection.PropertyInfo;
using static TeeSquare.Util.CaseHelper;

namespace TeeSquare.Reflection
{
    public class TypeConverter
    {
        public TypeConverter(
            params (Type type, string tsType)[] staticMappings)
        {
            foreach (var mapping in staticMappings)
            {
                _staticMappings[mapping.type] = mapping.tsType;
            }
        }

        public bool RemovePrefixFromInterfaces { get; set; } = true;
        public NamingConventions NamingConventions { get; set; } = NamingConventions.Default;

        private readonly IDictionary<Type, string> _staticMappings =
            new Dictionary<Type, string>
            {
                {typeof(string), "string"},
                {typeof(Guid), "string"},
                {typeof(void), "void"},
                {typeof(Decimal), "number"},
                {typeof(byte), "number"},
                {typeof(Int16), "number"},
                {typeof(Int32), "number"},
                {typeof(Int64), "number"},
                {typeof(double), "number"},
                {typeof(Single), "number"},
                {typeof(DateTime), "string"},
                {typeof(DateTimeOffset), "string"},
                {typeof(bool), "boolean"}
            };


        protected bool TryGetStaticMapping(Type type, out string tsType)
        {
            return _staticMappings.TryGetValue(type, out tsType);
        }

        /// <summary>
        /// Converts a c# type into a typescript type.
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <param name="parentType">If this type is from a property or a method param, the parentType is the type which has this property or method</param>
        /// <param name="info">PropertyInfo or MethodInfo if this is a property or method param</param>
        /// <returns></returns>
        public virtual ITypeReference Convert(Type type, Type parentType = null, MemberInfo info = null)
        {
            if (TryGetStaticMapping(type, out var mapping))
                return new TypeReference(mapping)
                {
                    ExistingType = true
                };
            if (type.IsTask(out var resultType))
            {
                return Convert(resultType, parentType, info);
            }

            if (type.IsEnum)
            {
                return new TypeReference(ToCase(type.Name, NamingConventions.Types))
                    {Enum = true};
            }

            if (type.IsNullable(out var underlyingType))
            {
                return Convert(underlyingType, parentType, info).MakeOptional();
            }

            if (type.IsDictionary(out var genericTypeParams))
                return new TypeReference(
                        $"{{ [key: {Convert(genericTypeParams[0], parentType, info).FullName}]: {Convert(genericTypeParams[1], parentType, info).FullName} }}")
                    ;

            if (type.IsCollection(out var itemType))
            {
                return Convert(itemType, parentType, info).MakeArray();
            }

            if (type.IsGenericType)
            {
                var nonGenericName = TypeName(type);
                return new TypeReference(nonGenericName, type.GetGenericArguments()
                    .Select(t => Convert(t, parentType, info))
                    .ToArray());
            }

            return new TypeReference(TypeName(type));
        }

        public virtual string PropertyName(PropertyInfo propertyInfo)
        {
            return ToCase(propertyInfo.Name, NamingConventions.Properties);
        }

        public virtual string MethodName(MethodInfo methodInfo)
        {
            return ToCase(methodInfo.Name, NamingConventions.Methods);
        }

        public virtual string TypeName(Type type)
        {
            var name = type.Name;
            if (type.IsGenericType)
                name = type.Name.Split('`').First();
            if (type.IsInterface && RemovePrefixFromInterfaces && name.StartsWith("I"))
                name = name.Substring(1);

            return ToCase(name, NamingConventions.Types);
        }


        public string EnumName(string name)
        {
            return ToCase(name, NamingConventions.EnumsMembers);
        }
    }
}
