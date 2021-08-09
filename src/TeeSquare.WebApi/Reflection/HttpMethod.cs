

namespace TeeSquare.WebApi.Reflection
{
    public class HttpMethod
    {
        private readonly string _method;

        private HttpMethod(string method)
        {
            _method = method;
        }
        public static HttpMethod Get => new HttpMethod("GET");
        public static HttpMethod Put => new HttpMethod("PUT");
        public static HttpMethod Delete => new HttpMethod("DELETE");
        public static HttpMethod Post => new HttpMethod("POST");
        public static HttpMethod Patch => new HttpMethod("PATCH");
        public static HttpMethod Options => new HttpMethod("OPTIONS");

        public bool HasRequestBody()
        {
            return this == Put || this == Post || this == Patch;
        }

        public override bool Equals(object? obj)
        {
            return obj is HttpMethod method && _method.Equals(method._method);
        }

        protected bool Equals(HttpMethod other)
        {
            return _method == other._method;
        }

        public override int GetHashCode()
        {
            return (_method != null ? _method.GetHashCode() : 0);
        }

        public static bool operator ==(HttpMethod a, HttpMethod b)
        {
            return a._method == b._method;
        }

        public static bool operator !=(HttpMethod a, HttpMethod b)
        {
            return !(a == b);
        }

        public string GetName()
        {
            switch (_method)
            {
                case "GET":
                    return "Get";
                case "POST":
                    return "Post";
                case "PUT":
                    return "Put";
                case "DELETE":
                    return "Delete";
                case "OPTIONS":
                    return "Options";
                case "PATCH":
                    return "Patch";
                default:
                    return string.Empty;
            }
        }
    }
}
