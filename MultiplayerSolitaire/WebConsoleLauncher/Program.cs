namespace WebConsoleLauncher
{
    using MSGWeb;
    using System.Threading.Tasks;

    class Program
    {
        static void Main()
        {
            System.Threading.CancellationTokenSource cancellationTokenSource = new System.Threading.CancellationTokenSource();
            Task serverTask = MSGWeb.Run(MSGWeb.Parameters.Default(), cancellationTokenSource.Token);

            while (!serverTask.IsCompleted)
            { 
            }
        }
    }
}
