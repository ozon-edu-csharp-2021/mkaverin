using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.StartupFilters;

namespace WebApi.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            _ = builder.ConfigureServices(services =>
            {
                _ = services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());

                _ = services.AddSingleton<IStartupFilter, LoggingStartupFilter>();

                _ = services.AddSingleton<IStartupFilter, TerminalStartupFilter>();

                _ = services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

                _ = services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OzonEdu.MerchandiseService", Version = "v1" });
                    options.CustomSchemaIds(x => x.FullName);
                });
            });
            return builder;
        }
    }
}