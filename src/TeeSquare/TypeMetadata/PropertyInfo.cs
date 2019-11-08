namespace TeeSquare.TypeMetadata
{
    public interface IPropertyInfo
    {
        string Name { get; }
        ITypeReference Type { get; }
    }

    class PropertyInfo : IPropertyInfo
    {
        public string Name { get; }
        public ITypeReference Type { get; }

        public PropertyInfo(string name, ITypeReference typeReference)
        {
            Name = name;
            Type = typeReference;
        }
    }
}
