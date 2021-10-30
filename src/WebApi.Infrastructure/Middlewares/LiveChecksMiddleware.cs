using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Middlewares
{
    class LiveChecksMiddleware
    {
        public LiveChecksMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync("Live");
        }
    }
}
