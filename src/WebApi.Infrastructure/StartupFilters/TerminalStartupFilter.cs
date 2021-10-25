using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WebApi.Infrastructure.Middlewares;
using System;

namespace WebApi.Infrastructure.StartupFilters
{
    public class TerminalStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                _ = app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
                _ = app.Map("/ready", builder => builder.UseMiddleware<HealthChecksMiddleware>());
                _ = app.Map("/live", builder => builder.UseMiddleware<HealthChecksMiddleware>());
                next(app);
            };
        }
    }
}