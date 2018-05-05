namespace TeeSquare.Writers
{
    class IdentifierInfo : IIdentifierInfo
    {
        public string Name { get; }
        public string Type { get; }
        public string[] GenericTypeParams { get; }

        public IdentifierInfo(string name, string type, string[] genericTypeParams)
        {
            Name = name;
            Type = type;
            GenericTypeParams = genericTypeParams;
        }
    }
}