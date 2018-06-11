namespace TeeSquare.TypeMetadata
{
    public interface IIdentifierInfo
    {
        string Name { get; }
        string Type { get; }
        string[] GenericTypeParams { get; }
    }
}