namespace TeeSquare.Reflection
{
    public class NamingConventions
    {
        public static NamingConventions Default => new NamingConventions();

        public NameConvention Properties { get; set; } = NameConvention.CamelCase;
        public NameConvention Methods { get; set; } = NameConvention.CamelCase;
        public NameConvention Types { get; set; } = NameConvention.PascalCase;
        public NameConvention EnumsMembers { get; set; } = NameConvention.PascalCase;
    }
}
