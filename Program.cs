using System.Net;
using System.Net.Sockets;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListenerBase listener = new MyHttpListener(IPAddress.Loopback);
            listener.Start();
        }
    }
}
