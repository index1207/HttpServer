namespace HttpServer
{
    public enum Method
    {
        Get,
        Post
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {
        public Method Method { get; private set; }
        public string Path { get; private set; }

        public RouteAttribute(Method method, string path)
        {
            Method = method;
            Path = path;
        }
    }
}
