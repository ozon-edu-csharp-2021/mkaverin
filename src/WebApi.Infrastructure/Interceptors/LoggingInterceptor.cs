using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace WebApi.Interceptors
{
    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;
        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation($"Request rpc logged:{Environment.NewLine}" +
                                       $"Route: {context.Method} " +
                                       $"StatusCode: { context.Status.StatusCode} " +
                                       $"Body: {requestJson} ");

            var sw = Stopwatch.StartNew();
            var response = base.UnaryServerHandler(request, context, continuation);
            sw.Stop();

            var responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation($"Response rpc logged:{Environment.NewLine}" +
                                         $"Route: {context.Method} " +
                                         $"StatusCode: { context.Status.StatusCode} " +
                                         $"Elapsed: {sw.Elapsed.TotalMilliseconds:0.0000} ms " +
                                         $"Body: {responseJson} ");
            return response;
        }
    }
}