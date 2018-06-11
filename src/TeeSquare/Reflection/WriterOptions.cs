using System.Reflection;

namespace TeeSquare.Reflection
{
    public class WriterOptions
    {
        public Namer Namer { get; set; } = Namer.Default;

        public BindingFlags PropertyFlags = BindingFlags.GetProperty
                                            | BindingFlags.Public
                                            | BindingFlags.Instance;

        public bool WriteEnumDescriptions { get; set; } = false;
        public bool WriteEnumDescriptionGetters { get; set; } = false;
        public string IndentChars { get; set; } = "  ";
    }
}