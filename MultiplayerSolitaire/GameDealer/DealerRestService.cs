namespace GameDealer
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Resources;

    class DealerRestService : IMiddleware
    {
        private static bool ServerIsRunning = true;
        private static CancellationTokenRegistration AppShutdownHandler;
        private static readonly ResourceManager ResourceManager = new ResourceManager("GameDealer.Properties.Resources", typeof(Program).Assembly);

        // use dependency injection to grab a reference to the hosting container's lifetime cancellation tokens
        public DealerRestService(IHostApplicationLifetime hostLifetime)
        {
            if (AppShutdownHandler.Token.Equals(CancellationToken.None))
            {
                AppShutdownHandler = hostLifetime.ApplicationStopping.Register(DealerRestService.ApplicationShutdownHandler);
            }
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (ServerIsRunning)
                {
                    if (context.Request.Headers["Accept"][0].Contains("text/html"))
                    {
                        Console.WriteLine("Sending HTML to client.");
                        string response = DealerRestService.GetIndexPage();
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(response);
                    }
                    else
                    {
                        string uri = context.Request.Path.ToUriComponent();
                        if (uri.EndsWith(".js"))
                        {
                            context.Response.ContentType = "application/javascript";
                            if (DealerRestService.TryGetFile(uri, out string stringResponse))
                            {
                                await context.Response.WriteAsync(stringResponse);
                            }
                        }
                        else if (uri.EndsWith(".css"))
                        {
                            context.Response.ContentType = "text/css";
                            if (DealerRestService.TryGetFile(uri, out string stringResponse))
                            {
                                await context.Response.WriteAsync(stringResponse);
                            }
                        }
                        else if (uri.EndsWith(".json"))
                        {
                            context.Response.ContentType = "text/json";
                            if (DealerRestService.TryGetFile(uri, out string stringResponse))
                            {
                                await context.Response.WriteAsync(stringResponse);
                            }
                        }

                        // ignore other requests (such as favicon)
                        // potentially other middleware will handle it (see finally block)
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

        // event-handlers are the sole case where async void is valid
        public static void ApplicationShutdownHandler()
        {
            DealerRestService.ServerIsRunning = false;
        }

        public static string GetIndexPage()
        {
            return DealerRestService.ResourceManager.GetString("index");
        }

        public static bool TryGetFile(string name, out string fileContent)
        {
            fileContent = string.Empty;

            string[] splitted = name.Split("/");
            string lastComponent = splitted[^1].Trim().Replace(".json", "").Replace(".js", "").Replace(".css", "");
            if (!string.IsNullOrEmpty(lastComponent))
            {
                fileContent = DealerRestService.ResourceManager.GetString(lastComponent);
                return !string.IsNullOrEmpty(fileContent);
            }

            return false;
        }
    }
}
