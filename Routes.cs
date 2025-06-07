using System.Net;

namespace EzWebServerV2
{
    public partial class Server
    {
        /// <summary>
        /// Creates a Get-route
        /// </summary>
        /// <param name="path">route ("/")</param>
        /// <param name="data">request and response</param>
        public void Get(string path, Action<Req, Res> handler)
        {
            getRoutes.Add(new RouteEntry(path, handler));
        }

        /// <summary>
        /// Creates a Post-route
        /// </summary>
        /// <param name="path">route ("/")</param>
        /// <param name="data">request and response</param>
        public void Post(string path, Action<Req, Res> handler)
        {
            postRoutes.Add(new RouteEntry(path, handler));
        }

        void Listen()
        {
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string reqPath = request.Url?.AbsolutePath ?? "";
                bool handled = false;
                string sid = Session.GetOrCreateSessionId(request, response);
                var sessionData = Session.GetSession(sid);

                if (request.HttpMethod == "GET")
                {
                    foreach (var route in getRoutes)
                    {
                        var match = MatchRoute(route.Path, reqPath, out var urlParams);
                        if (match)
                        {
                            var req = new Req(request, urlParams, sessionData);
                            var res = new Res(response);
                            route.Handler.Invoke(req, res);
                            handled = true;
                            break;
                        }
                    }
                }
                else if (request.HttpMethod == "POST")
                {
                    foreach (var route in postRoutes)
                    {
                        var match = MatchRoute(route.Path, reqPath, out var urlParams);
                        if (match)
                        {
                            var req = new Req(request, urlParams, sessionData);
                            var res = new Res(response);
                            route.Handler.Invoke(req, res);
                            handled = true;
                            break;
                        }
                    }
                }
                if (!handled)
                {
                    response.StatusCode = 404;
                    response.StatusDescription = "Not Found";
                    response.Close();
                }
            }
        }

        bool MatchRoute(string route, string actual, out Dictionary<string, string> urlParams)
        {
            urlParams = new();

            string[] routeParts = route.Trim('/').Split('/');
            string[] pathParts = actual.Trim('/').Split('/');

            if (routeParts.Length != pathParts.Length)
                return false;

            for (int i = 0; i < routeParts.Length; i++)
            {
                if (routeParts[i].StartsWith(":"))
                {
                    string key = routeParts[i].Substring(1);
                    urlParams[key] = pathParts[i];
                }
                else if (routeParts[i] != pathParts[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
