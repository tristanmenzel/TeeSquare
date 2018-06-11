namespace TeeSquare.TypeMetadata
{
    class ValueInfo
    {
        public ValueInfo(string name, int value, string description)
        {
            Name = name;
            Description = description;
            FormattedValue = value.ToString();
        }

        public ValueInfo(string name, string value, string description)
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