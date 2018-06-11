using System;
using System.Collections.Generic;
using System.Reflection;

namespace TeeSquare.Reflection
{
    public class Namer
    {
        public static Namer Default => new Namer();

        private readonly NamingConvensions _namingConvensions;

        private readonly IDictionary<Type, string> _staticMappings = new Dictionary<Type, string>
        {
            {typeof(string), "string"},
            {typeof(Decimal), "number"},
            {typeof(Int16), "number"},
            {typeof(Int32), "number"},
            {typeof(Int64), "number"},
            {typeof(double), "number"},
            {typeof(Single), "number"},
            {typeof(DateTime), "string"},
            {typeof(DateTimeOffset), "string"},
            {typeof(bool), "boolean"},
        };

        public Namer(NamingConvensions namingConvensions = null)
        {
            _namingConvensions = namingConvensions ?? NamingConvensions.Default;
        }

        public virtual string TypeName(Type type)
        {
            if (_staticMappings.TryGetValue(type, out var name)) return name;
            return ToCase(type.Name, _namingConvensions.Types);
        }

        public virtual string PropertyName(PropertyInfo propertyInfo)
        {
            return ToCase(propertyInfo.Name, _namingConvensions.Properties);
        }


        protected virtual string ToCase(string name, NameConvention nameConvention)
        {
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
            return ToCase(name, _namingConvensions.EnumsMembers);
        }
    }
}