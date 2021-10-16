namespace MSGWeb
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Runtime.CompilerServices;

    // Based on https://github.com/MV10/WebSocketExample
    // By Jon McGuire.

    public class Program
    {
        public const int CLOSE_SOCKET_TIMEOUT_MS = 2500;

        public static void Main(string[] args)
        {
            // Initialize game.
            GameProcess gp = GameProcess.Instance;

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new string[] { 
                        @"http://*:8080/",
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
    }
}
