namespace TeeSquare.TypeMetadata
{
    public interface IPropertyInfo
    {
        string Name { get; }
        string Type { get; }
        string[] GenericTypeParams { get; }
    }

    class PropertyInfo : IPropertyInfo
    {
        public string Name { get; }
        public string Type { get; }
        public string[] GenericTypeParams { get; }

        public PropertyInfo(string name, string type, string[] genericTypeParams)
        {
            Name = name;
            Type = type;
            GenericTypeParams = genericTypeParams;
        }
    }
}