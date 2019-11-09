namespace TeeSquare.TypeMetadata
{
    public interface IParamConfigurator
    {
        IParamConfigurator Param(string name, ITypeReference typeReference);
    }
}
