﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using WebApi.Infrastructure.Middlewares;

namespace WebApi.Infrastructure.StartupFilters
{
    class LoggingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                _ = app.UseMiddleware<RequestLoggingMiddleware>();
                _ = app.UseMiddleware<ResponseLoggingMiddleware>();
                next(app);
            };
        }
    }
}