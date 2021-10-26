using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Middlewares
{
    class ReadyChecksMiddleware
    {
        public ReadyChecksMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
{
            await context.Response.WriteAsync("Ready");
        }
    }
}
