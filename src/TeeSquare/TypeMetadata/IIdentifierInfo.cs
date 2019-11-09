namespace TeeSquare.TypeMetadata
{
    public interface IIdentifierInfo
    {
        string Name { get; }
        ITypeReference Type { get; }
    }
}
