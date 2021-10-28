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
                app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
                app.Map("/ready", builder => builder.UseMiddleware<ReadyChecksMiddleware>());
                app.Map("/live", builder => builder.UseMiddleware<LiveChecksMiddleware>());
                next(app);
            };
        }
    }
}