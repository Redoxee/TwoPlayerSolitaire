namespace MSGWeb
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class RestMiddleware : IMiddleware
    {
        private static bool ServerIsRunning = true;
        private static CancellationTokenRegistration AppShutdownHandler;

        // use dependency injection to grab a reference to the hosting container's lifetime cancellation tokens
        public RestMiddleware(IHostApplicationLifetime hostLifetime)
        {
            if (AppShutdownHandler.Token.Equals(CancellationToken.None))
            {
                AppShutdownHandler = hostLifetime.ApplicationStopping.Register(ApplicationShutdownHandler);
            }
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (ServerIsRunning)
                {
                    string requestStringPath = context.Request.Path.ToUriComponent();
                    if (context.Request.Method == "GET")
                    {
                        if (context.Request.Headers["Accept"][0].Contains("text/html"))
                        {
                            Console.WriteLine("Sending HTML to client.");
                            string response = RestRequestService.GetIndexPage();
                            context.Response.ContentType = "text/html";
                            await context.Response.WriteAsync(response);
                        }
                        else
                        {
                            if (requestStringPath.EndsWith(".js"))
                            {
                                context.Response.ContentType = "application/javascript";
                                if (RestRequestService.TryGetFile(requestStringPath, out string stringResponse))
                                {
                                    await context.Response.WriteAsync(stringResponse);
                                }
                            }
                            else if (requestStringPath.EndsWith(".css") ||
                                requestStringPath.EndsWith(".json") ||
                                requestStringPath.EndsWith(".txt"))
                            {
                                context.Response.ContentType = "text/css";
                                if (RestRequestService.TryGetFile(requestStringPath, out string stringResponse))
                                {
                                    await context.Response.WriteAsync(stringResponse);
                                }
                            }

                            // ignore other requests (such as favicon)
                            // potentially other middleware will handle it (see finally block)
                        }
                    }
                    else if (context.Request.Method == "POST")
                    {
                        if (requestStringPath.EndsWith("/RequestSave"))
                        {
                            string saveResult = SaveManager.Instance.RequestSave();
                            await context.Response.WriteAsync(saveResult);
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
                MSGWeb.ReportException(ex);
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
            RestMiddleware.ServerIsRunning = false;
        }
    }
}
