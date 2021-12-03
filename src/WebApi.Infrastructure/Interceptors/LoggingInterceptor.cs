using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
            _logger.LogInformation($"Request rpc logged:{Environment.NewLine}" +
                                       "Route: {@Method} " +
                                       "StatusCode: {@StatusCode} " +
                                       "Body: {@Request} ", context.Method, context.Status.StatusCode, request);

            var sw = Stopwatch.StartNew();
            var response = base.UnaryServerHandler(request, context, continuation);
            sw.Stop();

            _logger.LogInformation($"Response rpc logged:{Environment.NewLine}" +
                                         "Route: {@Method} " +
                                         "StatusCode: {@StatusCode} " +
                                         "Elapsed: {@Elapsed} ms " +
                                         "Body: {@ResponseJson} ", context.Method, context.Status.StatusCode, $"{sw.Elapsed.TotalMilliseconds:0.0000}", response);
            return response;
        }
    }
}