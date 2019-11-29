namespace TeeSquare.Mobx
{
    public interface IMobxOptions
    {
        string OptionalType { get; }
    }

    public class MobxOptions : IMobxOptions
    {
        public string OptionalType { get; set; } = "types.maybe";
    }
}
