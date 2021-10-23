namespace GameDealer
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Parameters parameters = Parameters.Default();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;

            Task serverTask = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new string[] {
                        $"http://{parameters.Domain}:{parameters.Port}/{parameters.EndPoint}",
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .RunAsync(cancellationToken);

            while (!serverTask.IsCompleted)
            {
            }
        }

        internal static void ReportException(Exception ex, [CallerMemberName] string location = "(Caller name not set)")
        {
            Console.WriteLine($"\n{location}:\n  Exception {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"  Inner Exception {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
        }

        public struct Parameters
        {
            public string Domain;
            public string Port;
            public string EndPoint;

            public static Parameters Default()
            {
                return new Parameters
                {
                    Domain = "*",
                    Port = "8080",
                    EndPoint = "",
                };
            }
        }
    }
}
