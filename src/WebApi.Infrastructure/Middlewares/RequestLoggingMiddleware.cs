using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);
            await _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            try
            {
                if (!IsGrpcRequest(context.Request))
                {
                    var headers = GetRequestHeaders(context);
                    var body = await GetRequestBody(context);

                    _logger.LogInformation($"Request logged:{Environment.NewLine}" +
                                           $"Route: {context.Request.Path} {Environment.NewLine}" +
                                           $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                                           $"Headers: {headers} " +
                                           $"Body: {body} ");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log request");
            }
        }
        private bool IsGrpcRequest(HttpRequest request) => request.ContentType == "application/grpc";

        private async Task<string> GetRequestBody(HttpContext context)
        {
            var bodyAsText = "Not body";
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering();

                var buffer = new byte[context.Request.ContentLength.Value];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                bodyAsText = Encoding.UTF8.GetString(buffer);
                context.Request.Body.Position = 0;
            }

            return bodyAsText;
        }
        private StringBuilder GetRequestHeaders(HttpContext context)
        {
            StringBuilder builder = new(Environment.NewLine);
            foreach (KeyValuePair<string, StringValues> header in context.Request.Headers)
            {
                builder.AppendLine($"\t {header.Key}:{header.Value}");
            }
            return builder;
        }
    }
}