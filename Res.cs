using System.Net;
using System.Text;
using System.Text.Json;

namespace EzWebServerV2
{
    public class Res
    {
        HttpListenerResponse response;

        int status = 200;

        public Res(HttpListenerResponse res)
        {
            response = res;
        }

        public Res Status(int code)
        {
            status = code;
            return this;
        }

        public void Text(string content)
        {
            Send(content, "text/plain; charset=utf-8");
        }

        public void Html(string content)
        {
            Send(content, "text/html; charset=utf-8");
        }

        public void Json(object obj)
        {
            string content = JsonSerializer.Serialize(obj);
            Send(content, "application/json; charset=utf-8");
        }

        public void SendFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Status(404).Text("404 Not Found");
                return;
            }

            string mime = GetMimeType(filePath);
            byte[] data = File.ReadAllBytes(filePath);

            response.StatusCode = status;
            response.ContentType = mime;
            response.ContentLength64 = data.Length;
            response.OutputStream.Write(data, 0, data.Length);
            response.OutputStream.Close();
        }

        void Send(string content, string contentType)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.StatusCode = status;
            response.ContentType = contentType;
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        string GetMimeType(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            return ext switch
            {
                ".html" => "text/html",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".json" => "application/json",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
