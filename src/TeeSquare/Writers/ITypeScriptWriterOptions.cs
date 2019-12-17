namespace TeeSquare.Writers
{
    public interface ITypeScriptWriterOptions
    {
        IInterfaceWriterFactory InterfaceWriterFactory { get; }
        IClassWriterFactory ClassWriterFactory { get; }
        IEnumWriterFactory EnumWriterFactory { get; }
        IFunctionWriterFactory FunctionWriterFactory { get; }
        string IndentCharacters { get; }
    }
}
