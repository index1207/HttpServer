using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace HttpServer
{
    public class HttpListenerBase
    {
        private Socket _socket;
        private Dictionary<string, Func<string?, Response>> _getMap = new();
        private Dictionary<string, Func<string?, Response>> _postMap = new();

        public HttpListenerBase(IPAddress address)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(address, 80));
            _socket.Listen();
        }

        public void Start()
        {
            MethodInfo[] methods = GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                List<RouteAttribute> attributes = method.GetCustomAttributes<RouteAttribute>(true).ToList();
                foreach (RouteAttribute attribute in attributes)
                {
                    switch (attribute.Method)
                    {
                        case Method.Get:
                            _getMap[attribute.Path] =
                                (Func<string?, Response>) method.CreateDelegate(typeof(Func<string?, Response>), this);
                            break;
                        case Method.Post:
                            _postMap[attribute.Path] =
                                (Func<string?, Response>) method.CreateDelegate(typeof(Func<string?, Response>), this);
                            break;
                    }
                }
            }

            Console.WriteLine($"[System] Http server running on {_socket.LocalEndPoint}");

            while (true)
            {
                Socket clientSocket = _socket.Accept();
                new HttpSession(clientSocket, this);
            }
        }

        public void RequestGet(HttpSession client, string path, string? query = null)
        {
            Func<string?, Response>? action = null;
            if (_getMap.TryGetValue(path, out action))
            {
                Response response = action(query);
                client.SendResponse(response);
            }
            else
            {
                var response = new Response()
                {
                    StatusCode = Status.NotFound,
                };
                client.SendResponse(response);
            }
        }

        public void RequestPost(HttpSession client, string path, string? body = null)
        {
            Func<string?, Response>? action = null;
            if (_postMap.TryGetValue(path, out action))
            {
                Response response = action(body);
                client.SendResponse(response);
            }
            else
            {
                Console.WriteLine($"[{path}] Failed to response");
            }
        }
    }
}
