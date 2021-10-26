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
            var originalBodyStream = context.Response.Body;

            try
            {
                if (context.Request.ContentType == null || (context.Request.ContentType != null && !context.Request.ContentType.Equals("application/grpc")))
                {
                   var  responseBody = "Not body";
                    using var memoryStream = new MemoryStream();
                    context.Response.Body = memoryStream;

                    var sw = Stopwatch.StartNew();
                    await _next(context);
                    sw.Stop();

                    memoryStream.Position = 0;
                    var reader = new StreamReader(memoryStream);
                     responseBody = await reader.ReadToEndAsync();

                    memoryStream.Position = 0;
                    await memoryStream.CopyToAsync(originalBodyStream);

                    StringBuilder builder = new(Environment.NewLine);
                    foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
                    {
                        _ = builder.AppendLine($"\t {header.Key}:{header.Value}");
                    }

                    _logger.LogInformation($"Response logged:{Environment.NewLine}" +
                                           $"Route: {context.Request.Path} {Environment.NewLine}" +
                                           $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                                           $"Elapsed: {sw.Elapsed.TotalMilliseconds:0.0000} ms {Environment.NewLine}" +
                                           $"Headers: {builder} " +
                                           $"Body: {responseBody}");
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }

            //try
            //{
            //    if (context.Request.ContentType == null || (context.Request.ContentType != null && !context.Request.ContentType.Equals("application/grpc")))
            //    {
            //        StringBuilder builder = new(Environment.NewLine);
            //        foreach (KeyValuePair<string, StringValues> header in context.Response.Headers)
            //        {
            //            _ = builder.AppendLine($"{header.Key}:{header.Value}");
            //        }

            //        _logger.LogInformation($"Response logged:{Environment.NewLine}" +
            //                               $"Route: {context.Request.Path} " +
            //                               $"QueryString: {context.Request.QueryString} " +
            //                               $"Headers: {builder} ");
            //    }
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, "Could not log response");
            //}
        }
    }
}