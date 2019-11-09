using System.Net.Http;

namespace TeeSquare.WebApi.Reflection
{
    public static class HttpMethodExtensions
    {
        public static bool HasRequestBody(this HttpMethod method)
        {
            return method == HttpMethod.Put || method == HttpMethod.Post;
        }

        public static string GetName(this HttpMethod method)
        {
            if (method == HttpMethod.Delete)
                return "Delete";

            if (method == HttpMethod.Post)
                return "Post";

            if (method == HttpMethod.Put)
                return "Put";

            return "Get";
        }
    }
}