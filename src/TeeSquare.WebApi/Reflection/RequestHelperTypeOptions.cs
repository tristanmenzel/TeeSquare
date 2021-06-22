namespace TeeSquare.WebApi.Reflection
{
    public class RequestHelperTypeOptions
    {
        private RequestHelperTypeOptions(bool emitTypes, string importFrom)
        {
            ShouldEmitTypes = emitTypes;
            ImportFrom = importFrom;
        }

        public string[] Types => new[]
        {
            "GetRequest",
            "PutRequest",
            "PostRequest",
            "PatchRequest",
            "OptionsRequest",
            "DeleteRequest",
            "toQuery"
        };

        public bool ShouldEmitTypes { get; }

        public string ImportFrom { get; }

        public static RequestHelperTypeOptions EmitTypes => new RequestHelperTypeOptions(true, null);

        public static RequestHelperTypeOptions ImportTypes(string importFrom) =>
            new RequestHelperTypeOptions(false, importFrom);
    }
}
