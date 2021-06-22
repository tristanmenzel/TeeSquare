namespace TeeSquare.Writers
{
    public interface ITypeScriptWriterOptions
    {
        IInterfaceWriterFactory InterfaceWriterFactory { get; set; }
        IClassWriterFactory ClassWriterFactory { get; set; }
        IEnumWriterFactory EnumWriterFactory { get; set; }
        IFunctionWriterFactory FunctionWriterFactory { get; set; }
        string IndentCharacters { get; set; }
    }
}
