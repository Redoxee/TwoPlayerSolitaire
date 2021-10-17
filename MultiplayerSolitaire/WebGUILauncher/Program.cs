
namespace WebGUILauncher
{
    using System;
    using System.Windows.Forms;
    using System.Threading;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using MSGWeb;

    static class Program
    {
        static CancellationTokenSource CancellationTokenSource;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerLauncher());
        }

        public static void LaunchGame(MSGWeb.Parameters parameters)
        {
            if (Program.CancellationTokenSource != null)
            {
                Console.WriteLine("Server already running");
                return;
            }

            Program.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            System.Threading.Tasks.Task.Run(()=>MSGWeb.Run(parameters, Program.CancellationTokenSource.Token));

            Program.OpenBrowser($"http://localhost:{parameters.Port}/{parameters.EndPoint}");
        }

        public static void CloseGame()
        {
            if (Program.CancellationTokenSource != null && !Program.CancellationTokenSource.IsCancellationRequested)
            {
                Program.CancellationTokenSource.Cancel();
            }
        }

        // From : https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
