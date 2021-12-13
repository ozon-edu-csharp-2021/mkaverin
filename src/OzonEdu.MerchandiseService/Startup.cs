using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseService.Extensions;

namespace OzonEdu.MerchandiseService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomOptions(Configuration)
                  .AddHostedServices()
                  .AddDatabaseConnection(Configuration)
                  .AddRepositories()
                  .AddApplicationServices()
                  .AddExternalServices(Configuration)
                  .AddKafkaServices(Configuration)
                  .AddOpenTracing()
                  .AddJaegerService(Configuration);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}