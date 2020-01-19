using System;
using System.Linq;

namespace TeeSquare.TypeMetadata
{
    public interface ITypeReference
    {
        string TypeName { get; }
        ITypeReference[] GenericTypeParams { get; }
        bool Enum { get; }
        bool Optional { get; }
        bool Array { get; }
        string FullName { get; }
        bool ExistingType { get; }
        ITypeReference MakeOptional(bool optional = true);
        ITypeReference MakeArray(bool array = true);
    }


    public class TypeReference : ITypeReference
    {
        public TypeReference(string typeName,
            ITypeReference[] genericTypeParams)
        {
            TypeName = typeName;
            GenericTypeParams = genericTypeParams;

        }

        public TypeReference(string typeName,
            params string[] genericTypeParams)
        {
            TypeName = typeName;
            GenericTypeParams = genericTypeParams.Select(t=> new TypeReference(t)).ToArray();
        }

        public string TypeName { get; set; }
        public ITypeReference[] GenericTypeParams { get; set; }
        public bool Enum { get; set; }
        public bool Optional { get; set; }
        public bool Array { get; set; }

        public string FullName => (GenericTypeParams.Any()
            ? $"{TypeName}<{string.Join(", ", GenericTypeParams.Select(p => p.FullName))}>"
            : TypeName) + (Array ? "[]": "");

        public bool ExistingType { get; set; }
        public ITypeReference MakeOptional(bool optional = true)
        {
            Optional = optional;
            return this;
        }

        public ITypeReference MakeArray(bool array = true)
        {
            Array = array;
            return this;
        }
    }
}
