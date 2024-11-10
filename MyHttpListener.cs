using System.Net;
using System.Text.Json;

namespace HttpServer
{
    public class MyHttpListener : HttpListenerBase
    {
        public MyHttpListener(IPAddress address) : base(address)
        {
        }

        [Route(Method.Get, "/")]
        public Response Index(string? query = null)
        {
            var response = new Response();
            response.Body = JsonSerializer.Serialize(new
            {
                Text = "Hello, World!"
            });
            response.ContentType = "application/json";
            return response;
        }
    }
}
