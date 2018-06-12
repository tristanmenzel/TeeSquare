namespace TeeSquare.TypeMetadata
{
    public interface IParamConfigurator
    {
        IParamConfigurator Param(string name, string type, params string[] genericTypeParams);
    }
}