namespace HttpServer
{
    public enum Status
    {
        Ok = 200,
        NotFound = 404
    }

    public class Response
    {
        // Status line
        public string HttpVersion { get; } = "HTTP/1.1";
        public Status StatusCode
        {
            get
            {
                return _statusCode;
            }
            set
            {
                _statusCode = value;
                switch (_statusCode)
                {
                    case Status.Ok:
                        StatusMessage = "OK";
                        break;
                    case Status.NotFound:
                        StatusMessage = "Not Found";
                        break;
                }
            }
        }
        public string StatusMessage { get; private set; } = string.Empty;

        // Headers
        public string ContentType { get; set; } = "text/html";
        public int ContentLength
        {
            get
            {
                return Body.Length;
            }
        }

        public string Body { get; set; } = string.Empty;

        private Status _statusCode = Status.Ok;

        public Response() { }

        public Response(string content)
        {
            Body = content;
        }

        public override string ToString()
        {
            return $"{HttpVersion} {Convert.ToInt32(StatusCode)} {StatusMessage}\r\n" +
                $"Content-Type: {ContentType}\r\n" +
                $"Content-Length: {ContentLength}\r\n" +
                $"\r\n" +
                $"{Body}";
        }
    }
}
