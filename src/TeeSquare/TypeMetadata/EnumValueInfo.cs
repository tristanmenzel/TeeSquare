namespace TeeSquare.TypeMetadata
{
    public interface IEnumValueInfo
    {
        string Name { get; }
        string Description { get; }
        string FormattedValue { get; }
    }

    class EnumValueInfo : IEnumValueInfo
    {
        public EnumValueInfo(string name, object value, string description)
        {
            Name = name;
            Description = description;
            FormattedValue = value.ToString();
        }

        public EnumValueInfo(string name, string value, string description)
        {
            Name = name;
            Description = description;
            FormattedValue = $"\"{value}\"";
        }

        public string Name { get; }
        public string Description { get; }
        public string FormattedValue { get; }
    }
}
