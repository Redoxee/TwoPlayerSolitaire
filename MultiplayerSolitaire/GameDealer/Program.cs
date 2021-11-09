namespace GameDealer
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Threading;

    internal class Program
    {
        public static void Main(string[] args)
        {
            Parameters parameters = Parameters.Default();
            int number = args?.Length ?? 0;
            for (int index = 0; index < number; ++index)
            {
                if (args[index] == "-p")
                {
                    parameters.DealerPort = args[index + 1];
                }
                else if (args[index] == "-d")
                {
                    parameters.Domain = args[index + 1];
                }
                else if (args[index] == "-i")
                {
                    parameters.GameImage = args[index + 1];
                }
                else if (args[index] == "-P")
                {
                    parameters.GamePort = ushort.Parse(args[index + 1]);
                }
            }

            Console.WriteLine($"Parameters {parameters.ToString()}");

            Dealer.Initialize(parameters);

            CancellationTokenSource tokenSource = new();
            CancellationToken cancellationToken = tokenSource.Token;

            Task serverTask = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new string[] {
                        $"http://{parameters.Domain}:{parameters.DealerPort}/{parameters.EndPoint}",
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
            public string DealerPort;
            public string EndPoint;
            public string GameImage;
            public ushort GamePort;

            public static Parameters Default()
            {
                return new Parameters
                {
                    Domain = "*",
                    DealerPort = "80",
                    EndPoint = "",
                    GameImage = "game_instance",
                    GamePort = 8081,
                };
            }

            public override string ToString()
            {
                return $"Domain {this.Domain} | DealerPort {this.DealerPort} | EndPoint {this.EndPoint} | GameImage {this.GameImage} | GamePort {this.GamePort}";
            }
        }

        // From https://stackoverflow.com/questions/3253701/get-public-external-ip-address
        public static System.Net.IPAddress GetPublicIp(string serviceUrl = "https://ipinfo.io/ip")
        {
            return System.Net.IPAddress.Parse(new System.Net.WebClient().DownloadString(serviceUrl));
        }
    }
}
