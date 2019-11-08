namespace TeeSquare.TypeMetadata
{
    class IdentifierInfo : IIdentifierInfo
    {
        public string Name { get; }
        public ITypeReference Type { get; }

        public IdentifierInfo(string name, ITypeReference type)
        {
            Name = name;
            Type = type;
        }
    }
}
