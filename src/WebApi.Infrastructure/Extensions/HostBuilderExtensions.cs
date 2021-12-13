using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.Infrastructure.Filters;
using WebApi.Infrastructure.StartupFilters;
using WebApi.Interceptors;

namespace WebApi.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());

                services.AddSingleton<IStartupFilter, LoggingStartupFilter>();

                services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());

                services.AddSingleton<IStartupFilter, TerminalStartupFilter>();

                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "OzonEdu.MerchandiseService", Version = "v1" });
                    options.CustomSchemaIds(x => x.FullName);
                });
            });
            return builder;
        }

    }

}