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
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);

        }
        private async Task LogRequest(HttpContext context)
        {
            try
            {
                if (!IsGrpcRequest(context.Request))
                {
                    var headers = GetHeaders(context.Request.Headers);
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

                    var headers = GetHeaders(context.Response.Headers);

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
        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{responseBody}";
        }
        private bool IsGrpcRequest(HttpRequest request) => request.ContentType == "application/grpc";
        private StringBuilder GetHeaders(IHeaderDictionary headers)
        {
            StringBuilder builder = new(Environment.NewLine);
            foreach (KeyValuePair<string, StringValues> header in headers)
            {
                builder.AppendLine($"\t {header.Key}:{header.Value}");
            }
            return builder;
        }
    }
}