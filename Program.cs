using System.Net;
using System.Net.Sockets;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListenerBase listener = new MyHttpListener(Dns.GetHostAddresses(Dns.GetHostName())[1]);
            listener.Start();
        }
    }
}
