using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Middlewares
{
    class HealthChecksMiddleware
    {
        public HealthChecksMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {

        }
    }
}
