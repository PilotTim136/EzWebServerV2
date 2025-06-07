using System.Net;

namespace EzWebServerV2
{
    public partial class Server
    {
        //PUBLIC

        /// <summary>
        /// Gets the listener, for custom or other scripts.
        /// </summary>
        public HttpListener listener { get; internal set; } = null!;

        /// <summary>
        /// Whether or not the server is listening to requests
        /// </summary>
        public bool listening => listener.IsListening || false;

        /// <summary>
        /// Whether or not it got a instruction to use threads
        /// </summary>
        public bool useThread { get; internal set; } = false;

        //PRIVATE
        List<RouteEntry> getRoutes = new();
        List<RouteEntry> postRoutes = new();
    }
    class RouteEntry
    {
        public string Path;
        public Action<Req, Res> Handler;

        public RouteEntry(string path, Action<Req, Res> handler)
        {
            Path = path;
            Handler = handler;
        }
    }
}
