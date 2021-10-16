namespace MSGWeb
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class RestMiddleware : IMiddleware
    {
        private static bool ServerIsRunning = true;
        private static CancellationTokenRegistration AppShutdownHandler;

        // use dependency injection to grab a reference to the hosting container's lifetime cancellation tokens
        public RestMiddleware(IHostApplicationLifetime hostLifetime)
        {
            // gracefully close all websockets during shutdown (only register on first instantiation)
            if (AppShutdownHandler.Token.Equals(CancellationToken.None))
                AppShutdownHandler = hostLifetime.ApplicationStopping.Register(ApplicationShutdownHandler);
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
                        string response = RestRequestService.HandleRestRequest();
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(response);
                    }
                    else 
                    {
                        string uri = context.Request.Path.ToUriComponent();
                        if (uri.EndsWith(".js"))
                        {
                            context.Response.ContentType = "application/javascript";
                            if (RestRequestService.TryGetFile(uri, out string stringResponse))
                            {
                                await context.Response.WriteAsync(stringResponse);
                            }
                        }
                        else if (uri.EndsWith(".css"))
                        {
                            context.Response.ContentType = "text/css";
                            if (RestRequestService.TryGetFile(uri, out string stringResponse))
                            {
                                await context.Response.WriteAsync(stringResponse);
                            }
                        }
                        else if (uri.EndsWith(".json"))
                        {
                            context.Response.ContentType = "text/css";
                            if (RestRequestService.TryGetFile(uri, out string stringResponse))
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
