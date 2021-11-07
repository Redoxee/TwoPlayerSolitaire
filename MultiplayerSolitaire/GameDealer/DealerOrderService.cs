namespace GameDealer
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    public class DealerOrderService : IMiddleware
    {
        private static bool ServerIsRunning = true;
        private static CancellationTokenRegistration AppShutdownHandler;

        private string publicAdresse;

        public DealerOrderService(IHostApplicationLifetime hostLifetime)
        {
            if (AppShutdownHandler.Token.Equals(CancellationToken.None))
            {
                AppShutdownHandler = hostLifetime.ApplicationStopping.Register(DealerOrderService.ApplicationShutdownHandler);
            }
            
            this.publicAdresse = Program.GetPublicIp().ToString();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (ServerIsRunning)
                {
                    if (context.Request.Method == "POST")
                    {
                        SimpleJSONBuilder responseBuilder = new SimpleJSONBuilder();
                        responseBuilder.Start();

                        context.Response.ContentType = "text/html";
                        System.IO.StreamReader requestStringReader = new System.IO.StreamReader(context.Request.Body);
                        string requestBodyLine = requestStringReader.ReadLine();
                        while (requestBodyLine != null)
                        {
                            if (requestBodyLine.StartsWith("Ticket:"))
                            {
                                responseBuilder.Add("Ticket", requestBodyLine.Replace("Ticket:", string.Empty));
                            }

                            requestBodyLine = requestStringReader.ReadLine();
                        }

                        string stringPath = context.Request.Path.ToUriComponent();
                        if (stringPath.EndsWith("RequestNewGame"))
                        {
                            Dealer.NewGameResult result;
                            Dealer.Instance.LaunchNewGame(out result);
                            responseBuilder.Add("GameLaunched", result.Success);

                            if (result.Success)
                            {
                                string externalAdress = $"{this.publicAdresse}:{result.Port}";
                                responseBuilder.Add("GameAdress", externalAdress);
                            }
                        }

                        string status = Dealer.Instance.GetStatus();
                        responseBuilder.Add("Status", status);

                        responseBuilder.End();
                        await context.Response.WriteAsync(responseBuilder.ToString());
                    }
                }
                else
                {
                    // ServerIsRunning = false
                    // HTTP 409 Conflict (with server's current state)
                    context.Response.StatusCode = 409;
                }
            }
            catch (Exception ex)
            {
                // HTTP 500 Internal server error
                context.Response.StatusCode = 500;
                Program.ReportException(ex);
            }
            finally
            {
                // if this middleware didn't handle the request, pass it on
                if (!context.Response.HasStarted)
                    await next(context);
            }
        }

        private static void ApplicationShutdownHandler()
        {
            DealerOrderService.ServerIsRunning = false;
        }
    }
}
