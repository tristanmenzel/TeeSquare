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
    public sealed class TypeConverter
    {
        public TypeConverter(params (Type type, string tsType)[] staticMappings)
        {
            EnumName = (member) => ToCase(member, NamingConventions.EnumsMembers);
            MethodName = (member) => ToCase(member.Name, NamingConventions.Methods);
            PropertyName = (member) => ToCase(member.Name, NamingConventions.Properties);
            FieldName = (member) => ToCase(member.Name, NamingConventions.Properties);

            TypeName = (type) =>
            {
                var name = type.Name;
                if (type.IsGenericType)
                    name = type.Name.Split('`').First();
                if (type.IsInterface && RemovePrefixFromInterfaces && name.StartsWith("I"))
                    name = name.Substring(1);

                return ToCase(name, NamingConventions.Types);
            };

            Convert = ConvertInternal;
            AddStaticMappings(staticMappings);
        }

        public void AddStaticMappings(params (Type type, string tsType)[] staticMappings)
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

        public bool HasStaticMapping(Type type)
        {
            return _staticMappings.ContainsKey(type);
        }

        public ConvertType Convert { get; set; }

        /// <summary>
        /// Converts a c# type into a typescript type.
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <param name="parentType">If this type is from a property or a method param, the parentType is the type which has this property or method</param>
        /// <param name="info">PropertyInfo or MethodInfo if this is a property or method param</param>
        /// <returns></returns>
        private ITypeReference ConvertInternal(Type type, Type parentType = null, MemberInfo info = null)
        {
            var isNullableReference = (info as PropertyInfo)?.IsNullableReference() ?? false;
            if (TryGetStaticMapping(type, out var mapping))
                return new TypeReference(mapping)
                {
                    ExistingType = true
                }.MakeOptional(isNullableReference);
            if (type.IsTask(out var resultType))
            {
                return ConvertInternal(resultType, parentType, info);
            }

            if (type.IsEnum)
            {
                return new TypeReference(ToCase(type.Name, NamingConventions.Types))
                    {Enum = true};
            }

            if (type.IsNullable(out var underlyingType))
            {
                return ConvertInternal(underlyingType, parentType, info).MakeOptional();
            }

            if (type.IsDictionary(out var genericTypeParams))
                return new TypeReference(
                        $"{{ [key: {ConvertInternal(genericTypeParams[0], parentType, info).FullName}]: {ConvertInternal(genericTypeParams[1], parentType, info).FullName} }}")
                    .MakeOptional(isNullableReference);

            if (type.IsCollection(out var itemType))
            {
                return ConvertInternal(itemType, parentType, info).MakeArray().MakeOptional(isNullableReference);
            }

            if (type.IsGenericType)
            {
                var nonGenericName = TypeName(type);
                return new TypeReference(nonGenericName, type.GetGenericArguments()
                        .Select(t => ConvertInternal(t, parentType, info))
                        .ToArray())
                    .MakeOptional(isNullableReference);
            }

            return new TypeReference(TypeName(type)).MakeOptional(isNullableReference);
        }

        public NameStrategy<Type> TypeName { get; set; }

        public NameStrategy<string> EnumName { get; set; }
        public NameStrategy<FieldInfo> FieldName { get; set; }
        public NameStrategy<MethodInfo> MethodName { get; set; }
        public NameStrategy<PropertyInfo> PropertyName { get; set; }
    }

    public delegate string NameStrategy<T>(T item);

    public delegate ITypeReference ConvertType(Type type, Type? parentType = null, MemberInfo? info = null);
}
