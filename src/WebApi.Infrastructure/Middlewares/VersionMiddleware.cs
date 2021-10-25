using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using WebApi.Infrastructure.Filters;

namespace WebApi.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }
        [GlobalExceptionFilter]
        public async Task InvokeAsync(HttpContext context)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "no version";
            var resultObject = new
            {
                version = version,
                serviceName = Assembly.GetEntryAssembly().GetName().Name
            };
            var jsonResult = JsonConvert.SerializeObject(resultObject);
            await context.Response.WriteAsync(jsonResult);
        }
    }
}