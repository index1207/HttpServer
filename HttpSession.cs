using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer
{
    public class HttpSession
    {
        private Socket _socket;
        private SocketAsyncEventArgs _recvEvent = new();

        private HttpListenerBase _listener;

        public HttpSession(Socket socket, HttpListenerBase listener)
        {
            _socket = socket;
            _listener = listener;

            _recvEvent.Completed += new EventHandler<SocketAsyncEventArgs>(OnReadCompleted);
            _recvEvent.SetBuffer(new byte[0x10000], 0, 0x10000);

            if (!_socket.ReceiveAsync(_recvEvent))
                OnReadCompleted(null, _recvEvent);
        }

        public void SendResponse(Response response)
        {
            _socket.Send(Encoding.UTF8.GetBytes(response.ToString()));
        }

        void OnReadCompleted(object? sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                string requestString = Encoding.UTF8.GetString(e.Buffer!, 0, e.BytesTransferred);
                string[] seperateRequest = requestString.Split("\r\n");

                if (seperateRequest.Length <= 0)
                    return;

                string startline = seperateRequest[0];
                string[] seperateStartline = startline.Split(' ');

                if (seperateStartline.Length < 2)
                    return;

                string method = seperateStartline[0];
                string path = seperateStartline[1];

                switch (method)
                {
                    case "GET":
                        _listener.RequestGet(this, path);
                        break;
                    case "POST":
                        _listener.RequestPost(this, path);
                        break;
                }

                if (!_socket.ReceiveAsync(_recvEvent))
                    OnReadCompleted(null, e);
            }
            else
            {
                _socket.Disconnect(true);
            }
        }
    }
}
