using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "no version";
            var resultObject = new
            {
                version = version,
                serviceName = Assembly.GetEntryAssembly().GetName().Name
            };
            string jsonResult = JsonConvert.SerializeObject(resultObject);
            await context.Response.WriteAsync(jsonResult);
        }
    }
}