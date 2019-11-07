using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TeeSquare.Reflection
{
    public class Namer
    {
        private readonly NamingConventions _namingConventions;

        private readonly IDictionary<Type, string> _staticMappings = new Dictionary<Type, string>
        {
            {typeof(string), "string"},
            {typeof(Guid), "string"},
            {typeof(void), "void"},
            {typeof(Decimal), "number"},
            {typeof(Int16), "number"},
            {typeof(Int32), "number"},
            {typeof(Int64), "number"},
            {typeof(double), "number"},
            {typeof(Single), "number"},
            {typeof(DateTime), "string"},
            {typeof(DateTimeOffset), "string"},
            {typeof(bool), "boolean"}
        };

        public virtual bool HasStaticMapping(Type type)
        {
            return _staticMappings.ContainsKey(type);
        }

        public virtual bool TryGetStaticMapping(Type type, out string name)
        {
            return _staticMappings.TryGetValue(type, out name);
        }

        public Namer(NamingConventions namingConventions = null)
        {
            _namingConventions = namingConventions ?? NamingConventions.Default;
        }

        public virtual string TypeName(Type type)
        {
            if (TryGetStaticMapping(type, out var name)) return name;
            if (type.IsTask(out var resultType))
            {
                return TypeName(resultType);
            }
            
            if (type.IsDictionary(out var genericTypeParams))
                return $"{{ [key: {TypeName(genericTypeParams[0])}]: {TypeName(genericTypeParams[1])} }}";

            if (type.IsCollection(out var itemType))
            {
                return $"{TypeName(itemType)}[]";
            }

            if (type.IsGenericType)
            {
                var nonGenericName = ToCase(type.Name.Split("`").First(), _namingConventions.Types);
                return $"{nonGenericName}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
            }

            return ToCase(type.Name, _namingConventions.Types);
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
