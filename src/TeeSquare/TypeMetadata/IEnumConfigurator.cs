namespace TeeSquare.TypeMetadata
{
    public interface IEnumConfigurator
    {
        void Value(string name, int value, string description = null);
        void Value(string name, string value, string description = null);
    }
}