using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TeeSquare.TypeMetadata;
using MethodInfo = System.Reflection.MethodInfo;
using PropertyInfo = System.Reflection.PropertyInfo;

namespace TeeSquare.Reflection
{
    public class Namer
    {
        private readonly NamingConventions _namingConventions;

        private readonly IDictionary<Type, (string tsType, TsTypeFormat format)> _staticMappings =
            new Dictionary<Type, (string tsType, TsTypeFormat format)>
            {
                {typeof(string), ("string", TsTypeFormat.None)},
                {typeof(Guid), ("string", TsTypeFormat.Guid)},
                {typeof(void), ("void", TsTypeFormat.None)},
                {typeof(Decimal), ("number", TsTypeFormat.Decimal)},
                {typeof(Int16), ("number", TsTypeFormat.Integer)},
                {typeof(Int32), ("number", TsTypeFormat.Integer)},
                {typeof(Int64), ("number", TsTypeFormat.Integer)},
                {typeof(double), ("number", TsTypeFormat.Decimal)},
                {typeof(Single), ("number", TsTypeFormat.Decimal)},
                {typeof(DateTime), ("string", TsTypeFormat.DateTime)},
                {typeof(DateTimeOffset), ("string", TsTypeFormat.DateTime)},
                {typeof(bool), ("boolean", TsTypeFormat.None)}
            };

        public virtual bool HasStaticMapping(Type type)
        {
            return _staticMappings.ContainsKey(type);
        }

        public virtual bool TryGetStaticMapping(Type type, out (string tsType, TsTypeFormat format) typeMapping)
        {
            return _staticMappings.TryGetValue(type, out typeMapping);
        }

        public Namer(NamingConventions namingConventions = null)
        {
            _namingConventions = namingConventions ?? NamingConventions.Default;
        }

        public virtual ITypeReference Type(Type type, bool optional = false)
        {
            if (TryGetStaticMapping(type, out var mapping)) return new TypeReference(mapping.tsType) {Optional = optional, Format = mapping.format};
            if (type.IsTask(out var resultType))
            {
                return Type(resultType, optional);
            }

            if (type.IsEnum)
            {
                return new TypeReference(ToCase(type.Name, _namingConventions.Types))
                    {Enum = true, Optional = optional};
            }

            if (type.IsNullable(out var underlyingType))
            {
                return Type(underlyingType, true);
            }

            if (type.IsDictionary(out var genericTypeParams))
                return new TypeReference(
                        $"{{ [key: {Type(genericTypeParams[0]).FullName}]: {Type(genericTypeParams[1]).FullName} }}")
                    {Optional = optional};

            if (type.IsCollection(out var itemType))
            {
                return new TypeReference($"{Type(itemType).FullName}") {Array = true, Optional = optional};
            }

            if (type.IsGenericType)
            {
                var nonGenericName = ToCase(type.Name.Split("`").First(), _namingConventions.Types);
                return new TypeReference(nonGenericName, type.GetGenericArguments()
                    .Select(t => Type(t))
                    .ToArray())
                {
                    Optional = optional
                };
            }

            return new TypeReference(ToCase(type.Name, _namingConventions.Types))
            {
                Optional = optional
            };
        }

        public virtual string PropertyName(PropertyInfo propertyInfo)
        {
            return ToCase(propertyInfo.Name, _namingConventions.Properties);
        }

        public virtual string MethodName(MethodInfo methodInfo)
        {
            return ToCase(methodInfo.Name, _namingConventions.Methods);
        }


        protected virtual string ToCase(string name, NameConvention nameConvention)
        {
            if (string.IsNullOrEmpty(name)) return name;
            name = new Regex(@"\-([a-z]?)", RegexOptions.IgnoreCase)
                .Replace(name, m => m.Groups[1].Value.ToUpper());
            switch (nameConvention)
            {
                case NameConvention.CamelCase:
                    return $"{name.Substring(0, 1).ToLower()}{name.Substring(1)}";
                case NameConvention.PascalCase:
                    return $"{name.Substring(0, 1).ToUpper()}{name.Substring(1)}";
                default:
                    return name;
            }
        }

        public string EnumName(string name)
        {
            return ToCase(name, _namingConventions.EnumsMembers);
        }
    }
}
