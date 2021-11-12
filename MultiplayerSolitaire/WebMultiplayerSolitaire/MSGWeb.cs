namespace MSGWeb
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Threading;

    // Based on https://github.com/MV10/WebSocketExample
    // By Jon McGuire.

    public class MSGWeb
    {
        public const int CLOSE_SOCKET_TIMEOUT_MS = 2500;

        public static async Task Run(Parameters parameters, CancellationToken cancellationToken)
        {
            // Initialize game.
            GameProcess.Initialize(parameters.GameParameters);
            cancellationToken.Register(MSGWeb.OnCancellation);

            if (parameters.OnEveryClientDisconected != null)
            {
                WebSocketMiddleware.AllClientClosed += parameters.OnEveryClientDisconected;
            }

            await Host.CreateDefaultBuilder(parameters.HostArgs)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new string[] { 
                        parameters.Url,
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .RunAsync(cancellationToken);
        }

        public static MSG.GameManager GameManager => GameProcess.Instance.GetGameManager();

        internal static void ReportException(Exception ex, [CallerMemberName] string location = "(Caller name not set)")
        {
            Console.WriteLine($"\n{location}:\n  Exception {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"  Inner Exception {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
        }

        internal static void OnCancellation()
        {
            Console.WriteLine("MGSWeb cancelled");
        }

        public struct Parameters
        {
            public string Domain;
            public string Port;
            public string EndPoint;
            public string[] HostArgs;
            public MSG.GameManager.GameParameters GameParameters;
            public Action OnEveryClientDisconected;

            public static Parameters Default()
            {
                return new Parameters
                {
                    Domain = "*",
                    Port = "8080",
                    EndPoint = string.Empty,
                    HostArgs = System.Array.Empty<string>(),
                    GameParameters = MSG.GameManager.GameParameters.Default(),
                    OnEveryClientDisconected = null,
                };
            }

            public string Url => $@"http://{this.Domain}:{this.Port}/{this.EndPoint}";
        }
    }
}
