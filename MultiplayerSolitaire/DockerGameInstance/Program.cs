namespace NetCore.Docker
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
                if (args[index] == "-p")
                {
                    parameter.Port = args[index + 1];
                }
                else if (args[index] == "-d")
                {
                    parameter.Domain = args[index + 1];
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
