using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            return new Response("Hi");
        }
    }
}
