using System.Net;

namespace EzWebServerV2
{
    internal class Session
    {
        static Dictionary<string, Dictionary<string, object>> sessions = new();
        internal static string CookieName = "sid";

        internal static string GetOrCreateSessionId(HttpListenerRequest req, HttpListenerResponse res)
        {
            string? cookie = req.Headers["Cookie"];
            string? sid = null;

            if (!string.IsNullOrWhiteSpace(cookie))
            {
                foreach (var part in cookie.Split(';'))
                {
                    var trimmed = part.Trim();
                    if (trimmed.StartsWith(CookieName + "="))
                    {
                        sid = trimmed.Substring(CookieName.Length + 1);
                        break;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(sid) || !sessions.ContainsKey(sid))
            {
                sid = GenerateSessionId();
                sessions[sid] = new Dictionary<string, object>();
                res.Headers.Add("Set-Cookie", $"{CookieName}={sid}; Path=/; HttpOnly");
            }

            return sid;
        }

        internal static Dictionary<string, object> GetSession(string sid)
        {
            return sessions.TryGetValue(sid, out var session) ? session : new();
        }

        static string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
