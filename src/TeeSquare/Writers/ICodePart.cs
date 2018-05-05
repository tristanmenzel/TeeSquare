namespace TeeSquare.Writers
{
    public interface ICodePart
    {
        void WriteTo(ICodeWriter writer);
    }
}