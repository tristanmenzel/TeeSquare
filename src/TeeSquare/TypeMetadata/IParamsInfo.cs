namespace TeeSquare.TypeMetadata
{
    public interface IParamsInfo
    {
        IParamsInfo Param(string name, string type, params string[] genericTypeParams);
    }
}