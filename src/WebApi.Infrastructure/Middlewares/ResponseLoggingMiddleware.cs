using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Infrastructure.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<ResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogResponse(context);
        }

        private async Task LogResponse(HttpContext context)
        {
            if (!IsGrpcRequest(context.Request))
            {
                var originalBodyStream = context.Response.Body;
                try
                {
                    using var memoryStream = new MemoryStream();
                    context.Response.Body = memoryStream;

                    var sw = Stopwatch.StartNew();
                    await _next(context);
                    sw.Stop();

                    var responseBody = await GetResponseBody(context.Response);
                    await memoryStream.CopyToAsync(originalBodyStream);

                    var headers = GetResponseHeaders(context);

                    _logger.LogInformation($"Response logged:{Environment.NewLine}" +
                                           $"Route: {context.Request.Path} {Environment.NewLine}" +
                                           $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                                           $"Elapsed: {sw.Elapsed.TotalMilliseconds:0.0000} ms {Environment.NewLine}" +
                                           $"Headers: {headers} " +
                                           $"Body: {responseBody}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not log response");
                }
                finally
                {
                    context.Response.Body = originalBodyStream;
                }
            }
        }
        private bool IsGrpcRequest(HttpRequest request) => request.ContentType == "application/grpc";

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{responseBody}";
        }
        private StringBuilder GetResponseHeaders(HttpContext context)
        {
            StringBuilder builder = new(Environment.NewLine);
            foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
            {
                builder.AppendLine($"\t {header.Key}:{header.Value}");
            }

            return builder;
        }
    }
}