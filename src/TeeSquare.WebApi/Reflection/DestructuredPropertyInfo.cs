namespace TeeSquare.WebApi.Reflection
{
    public class DestructuredPropertyInfo
    {
        public string DestructureAs { get; set; }
        public string PropertyName { get; set; }

        public string DestructureExpression =>
            DestructureAs == PropertyName ? PropertyName : $"{DestructureAs}: {PropertyName}";
    }
}
