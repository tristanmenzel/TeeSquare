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
        ITypeReference MakePartial(bool partial = true);
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
            GenericTypeParams = genericTypeParams.Select(t => new TypeReference(t)).ToArray();
        }

        public string TypeName { get; set; }
        public ITypeReference[] GenericTypeParams { get; set; }
        public bool Enum { get; set; }
        public bool Optional { get; set; }
        public bool Partial { get; set; }
        public bool Array { get; set; }

        public string FullName => WrapIfPartial(NameIncludingGenerics, Partial) + (Array ? "[]" : "");

        private static string WrapIfPartial(string type, bool partial)
        {
            return partial ? $"Partial<{type}>" : type;
        }

        private string NameIncludingGenerics => GenericTypeParams.Any()
            ? $"{TypeName}<{string.Join(", ", GenericTypeParams.Select(p => p.FullName))}>"
            : TypeName;

        public bool ExistingType { get; set; }

        public ITypeReference MakeOptional(bool optional = true)
        {
            Optional = optional;
            return this;
        }

        public ITypeReference MakePartial(bool partial = true)
        {
            Partial = partial;
            return this;
        }

        public ITypeReference MakeArray(bool array = true)
        {
            Array = array;
            return this;
        }
    }
}
