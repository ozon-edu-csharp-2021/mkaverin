using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.GrpcServices;
using OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate;
using OzonEdu.MerchandiseService.ApplicationServices.Stubs;

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
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);
            services.AddScoped<IOrderRepository, OrderRepositoryStub>();
            services.AddScoped<IMerchPackRepository, MerchPackRepositoryStub>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
              {
                  endpoints.MapGrpcService<MerchandiseGrpService>();
                  endpoints.MapControllers();
              });
        }
    }
}