using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Web;

namespace EzWebServerV2
{
    public class Req
    {
        HttpListenerRequest request;
        public HttpListenerRequest raw => request;

        public Dictionary<string, string> body { get; internal set; } = new();
        //public Dictionary<string, object> jsonBody { get; internal set; } = new();
        //public Dictionary<string, string> param { get; internal set; } = new();
        //public Dictionary<string, string> query { get; internal set; } = new();

        Dictionary<string, object> jsonBody = new();
        Dictionary<string, string> param = new();
        Dictionary<string, string> query = new();


        Dictionary<string, object> session = new();

        public Req(HttpListenerRequest req, Dictionary<string, string> param, Dictionary<string, object> sessionData)
        {
            request = req;
            this.param = param;
            session = sessionData;
            Create();
        }

        public object Session(string key)
        {
            return session.TryGetValue(key, out var value) ? value : "";
        }
        public void Session(string key, object value)
        {
            session[key] = value;
        }
        public string Query(string key)
        {
            return query.TryGetValue(key, out var value) ? value : "";
        }
        public string Param(string key)
        {
            return param.TryGetValue(key, out var value) ? value : "";
        }
        public string Body(string key)
        {
            return body.TryGetValue(key, out var value) ? value : "";
        }
        public object? GetJsonBodyValue(string key)
        {
            return jsonBody.TryGetValue(key, out object? value) ? value : null;
        }

        void Create()
        {
            NameValueCollection query = HttpUtility.ParseQueryString(request.Url?.Query ?? "");
            foreach (string? key in query.AllKeys!)
            {
                if (key != null)
                    this.query[key] = query[key]!;
            }

            string contentType = request.ContentType ?? "";

            if (request.HttpMethod == "POST" && request.HasEntityBody)
            {
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                string raw = reader.ReadToEnd();

                if (contentType.Contains("application/json"))
                {
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<Dictionary<string, object>>(raw);
                        jsonBody = parsed!;
                        foreach (var kv in parsed!)
                            body[kv.Key] = kv.Value?.ToString() ?? "";
                    }
                    catch{}
                }
                else if (contentType.Contains("application/x-www-form-urlencoded"))
                {
                    var pairs = raw.Split('&');
                    foreach (var pair in pairs)
                    {
                        var kv = pair.Split('=');
                        if (kv.Length == 2)
                        {
                            string k = Uri.UnescapeDataString(kv[0]);
                            string v = Uri.UnescapeDataString(kv[1]);
                            body[k] = v;
                        }
                    }
                }
            }
        }
    }
}
