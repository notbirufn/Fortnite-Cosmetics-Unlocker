using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fortnite_Cosmetics_Unlocker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!Fiddler.Setup())
            {
                return;
            }

            FiddlerCoreStartupSettings startupSettings = new FiddlerCoreStartupSettingsBuilder().ListenOnPort(9999).DecryptSSL().RegisterAsSystemProxy().Build();

            FiddlerApplication.BeforeRequest += OnBeforeRequest;
            FiddlerApplication.BeforeResponse += OnBeforeResponse;

            FiddlerApplication.Startup(startupSettings);

            Backend.Listen();

            Console.ReadKey(true);

            FiddlerApplication.Shutdown();

            Environment.Exit(0);
        }

        private static void OnBeforeRequest(Session session)
        {
            if (session.RequestHeaders["User-Agent"].Split('/')[0] == "Fortnite")
            {
                if (session.HTTPMethodIs("CONNECT"))
                {
                    session["x-replywithtunnel"] = "FortniteTunnel";
                    return;
                }

                if (session.PathAndQuery.StartsWith("/fortnite/api/game/v2/profile/") || session.PathAndQuery.StartsWith("/api/locker/v4/"))
                {
                    session.fullUrl = "http://localhost:1911" + session.PathAndQuery;
                }
            }
        }

        private static void OnBeforeResponse(Session session)
        {

        }
    }
}
