namespace TeeSquare.Reflection
{
    public class NamingConvensions
    {
        public static NamingConvensions Default => new NamingConvensions();

        public NameConvention Properties { get; set; } = NameConvention.CamelCase;
        public NameConvention Methods { get; set; } = NameConvention.CamelCase;
        public NameConvention Types { get; set; } = NameConvention.PascalCase;
        public NameConvention EnumsMembers { get; set; } = NameConvention.PascalCase;
    }
}