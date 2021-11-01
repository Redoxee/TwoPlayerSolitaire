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
        public DealerOrderService(IHostApplicationLifetime hostLifetime)
        {
            if (AppShutdownHandler.Token.Equals(CancellationToken.None))
            {
                AppShutdownHandler = hostLifetime.ApplicationStopping.Register(DealerOrderService.ApplicationShutdownHandler);
            }
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (ServerIsRunning)
                {
                    if (context.Request.Method == "POST")
                    {
                        string stringPath = context.Request.Path.ToUriComponent();
                        if (stringPath.EndsWith("RequestNewGame"))
                        {
                            context.Response.ContentType = "text/html";
                            Dealer.Instance.LaunchNewGame();

                            await context.Response.WriteAsync("Game started at address x.");
                        }
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
