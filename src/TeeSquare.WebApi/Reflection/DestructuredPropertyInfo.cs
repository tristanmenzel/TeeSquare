namespace TeeSquare.WebApi.Reflection
{
    public class DestructuredPropertyInfo
    {
        public DestructuredPropertyInfo(string destructureAs, string propertyName)
        {
            DestructureAs = destructureAs;
            PropertyName = propertyName;
        }
        public string DestructureAs { get;  }
        public string PropertyName { get; }

        public string DestructureExpression =>
            DestructureAs == PropertyName ? PropertyName : $"{DestructureAs}: {PropertyName}";
    }
}
