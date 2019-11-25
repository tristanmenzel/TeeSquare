using System;
using System.Collections.Generic;
using System.Linq;

namespace TeeSquare.Reflection
{
    public class TypeCollection
    {
        private readonly IDictionary<Type, TypeCollectionItem> _items = new Dictionary<Type, TypeCollectionItem>();

        private readonly List<LiteralImport> _literalImports = new List<LiteralImport>();
        public bool Contains(Type type)
        {
            return _items.ContainsKey(type);
        }

        public void AddImported(string path, Type type)
        {
            _items.Add(type, new ImportedType(type, path));
        }

        public void AddLocal(Type type)
        {
            _items.Add(type, new LocalType(type));
        }

        public void AddLiteralImport(string path, string type, string importAs = null)
        {
            _literalImports.Add(new LiteralImport(type, path, importAs));
        }

        public LiteralImport[] ImportedLiterals => _literalImports.ToArray();
        public ImportedType[] ImportedTypes => _items.Values.OfType<ImportedType>().ToArray();
        public Type[] LocalTypes => _items.Values.OfType<LocalType>().Select(t => t.Type).ToArray();
    }

    public abstract class TypeCollectionItem
    {
        protected TypeCollectionItem(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }

    public class LiteralImport
    {
        public LiteralImport(string typeName, string importFrom, string importAs = null)
        {
            TypeName = typeName;
            ImportAs = importAs;
            ImportFrom = importFrom;
        }

        public string TypeName { get; }
        public string ImportAs { get; }
        public string ImportFrom { get; }
    }

    public class LocalType : TypeCollectionItem
    {
        public LocalType(Type type) : base(type)
        {
        }
    }

    public class ImportedType : TypeCollectionItem
    {
        public ImportedType(Type type, string importFrom) : base(type)
        {
            ImportFrom = importFrom;
        }

        public string ImportFrom { get; }

    }
}
