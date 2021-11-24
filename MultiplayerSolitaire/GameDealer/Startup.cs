namespace GameDealer
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // register our custom middleware since we use the IMiddleware factory approach
            services.AddTransient<DealerRestService>();
            services.AddTransient<DealerOrderService>();

            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(option =>
            {
                option.AllowSynchronousIO = true;
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment _)
        {
            // add our custom middleware to the pipeline
            app.UseMiddleware<DealerRestService>();
            app.UseMiddleware<DealerOrderService>();
        }
    }
}
