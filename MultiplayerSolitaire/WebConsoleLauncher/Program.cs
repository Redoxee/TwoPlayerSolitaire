namespace WebConsoleLauncher
{
    using MSGWeb;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.CancellationTokenSource cancellationTokenSource = new();
            MSGWeb.Parameters parameter = MSGWeb.Parameters.Default();

            int number = args?.Length ?? 0;
            for (int index = 0; index < number; ++index)
            {
                switch (args[index])
                {
                    case "-p":
                        {
                            parameter.Port = args[index + 1];
                            break;
                        }
                    case "-d":
                        {
                            parameter.Domain = args[index + 1];
                            break;
                        }
                    case "--save-path":
                        {
                            parameter.SavePath = args[index + 1];
                            break;
                        }
                    case "--save-load":
                        {
                            parameter.LoadSavePath = args[index + 1];
                            break;
                        }
                }
            }

            parameter.OnEveryClientDisconected += () => {
                cancellationTokenSource.Cancel();
            };

            Task serverTask = MSGWeb.Run(parameter, cancellationTokenSource.Token);

            while (!serverTask.IsCompleted)
            {
            }
        }
    }
}
