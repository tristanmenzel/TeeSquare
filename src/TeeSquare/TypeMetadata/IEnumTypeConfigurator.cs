namespace TeeSquare.Writers
{
    public interface IEnumTypeConfigurator
    {
        void Value(string name, int value, string description = null);
        void Value(string name, string value, string description = null);
    }
}