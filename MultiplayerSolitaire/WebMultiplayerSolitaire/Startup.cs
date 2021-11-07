namespace MSGWeb
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    internal class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // register our custom middleware since we use the IMiddleware factory approach
            services.AddTransient<WebSocketMiddleware>();
            services.AddTransient<RestMiddleware>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            // enable websocket support
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            });

            // add our custom middleware to the pipeline
            app.UseMiddleware<WebSocketMiddleware>();
            app.UseMiddleware<RestMiddleware>();
        }
    }
}
