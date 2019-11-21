namespace TeeSquare.Writers
{
    public class TypeScriptWriterOptions : ITypeScriptWriterOptions
    {
        public IInterfaceWriterFactory InterfaceWriterFactory { get; set; } = new InterfaceWriterFactory();
        public IClassWriterFactory ClassWriterFactory { get; set; } = new ClassWriterFactory();
        public IEnumWriterFactory EnumWriterFactory { get; set; } = new EnumWriterFactory();
        public IFunctionWriterFactory FunctionWriterFactory { get; set; } = new FunctionWriterFactory();
        public string IndentCharacters { get; set; } = "  ";
    }
}
