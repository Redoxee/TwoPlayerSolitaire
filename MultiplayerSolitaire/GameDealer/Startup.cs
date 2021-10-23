namespace GameDealer
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // register our custom middleware since we use the IMiddleware factory approach
            services.AddTransient<DealerRestService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // add our custom middleware to the pipeline
            app.UseMiddleware<DealerRestService>();
        }
    }
}
