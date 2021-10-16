namespace MSGWeb
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Runtime.CompilerServices;

    // Based on https://github.com/MV10/WebSocketExample
    // By Jon McGuire.

    public class MSGWeb
    {
        public const int CLOSE_SOCKET_TIMEOUT_MS = 2500;

        public static void Run(Parameters parameters)
        {
            // Initialize game.
            GameProcess gp = GameProcess.Instance;

            Host.CreateDefaultBuilder(parameters.HostArgs)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new string[] { 
                        $@"http://{parameters.EndPoint}:{parameters.Port}/",
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .Run();
        }

        public static void ReportException(Exception ex, [CallerMemberName] string location = "(Caller name not set)")
        {
            Console.WriteLine($"\n{location}:\n  Exception {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"  Inner Exception {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
        }

        public struct Parameters
        {
            public string EndPoint;
            public string Port;
            public string[] HostArgs;
            
            public static Parameters Default()
            {
                return new Parameters
                {
                    EndPoint = "*",
                    Port = "8080",
                    HostArgs = System.Array.Empty<string>(),
                };
            }
        }
    }
}
