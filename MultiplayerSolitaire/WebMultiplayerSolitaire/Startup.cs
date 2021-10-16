using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MSGWeb
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            // register our custom middleware since we use the IMiddleware factory approach
            services.AddTransient<WebSocketMiddleware>();
            services.AddTransient<RestMiddleware>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // enable websocket support
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 64
            });

            // add our custom middleware to the pipeline
            app.UseMiddleware<WebSocketMiddleware>();
            app.UseMiddleware<RestMiddleware>();
        }
    }
}
