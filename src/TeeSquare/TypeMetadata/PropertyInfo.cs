namespace TeeSquare.Writers
{
    class PropertyInfo
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