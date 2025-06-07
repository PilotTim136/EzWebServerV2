using System.Net;

namespace EzWebServerV2
{
    public partial class Server
    {
        /// <summary>
        /// Create the web server.
        /// </summary>
        /// <param name="url">How the web server will be created</param>
        public Server(string url)
        {
            Create(url);
        }
        /// <summary>
        /// Create the web server.
        /// </summary>
        /// <param name="url">How the web server will be created</param>
        /// <param name="createThread">If it should start a new thread for the server</param>
        public Server(string url, bool createThread)
        {
            Create(url);
            useThread = createThread;
        }

        void Create(string url)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
        }

        /// <summary>
        /// Starts the web-server.
        /// NOTICE: Create GET and POST requests first!
        /// </summary>
        public void Start()
        {
            listener.Start();
            if (useThread)
            {
                Task.Run(Listen);
            }
            else
            {
                Listen();
            }
        }
    }
}
