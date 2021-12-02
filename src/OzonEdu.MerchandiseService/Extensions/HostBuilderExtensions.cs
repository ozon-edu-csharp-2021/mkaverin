using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace OzonEdu.MerchandiseService.Extensions
{
    internal static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigurePorts(this IHostBuilder builder)
        {

            var httpPortEnv = Environment.GetEnvironmentVariable("HTTP_PORT");
            if (!int.TryParse(httpPortEnv, out var httpPort))
            {
                httpPort = 5000;
            }

            var grpcPortEnv = Environment.GetEnvironmentVariable("GRPC_PORT");
            if (!int.TryParse(grpcPortEnv, out var grpcPort))
            {
                grpcPort = 5002;
            }

            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                httpPort = 80;
                grpcPort = 82;
            }

            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(
                    options =>
                    {
                        Listen(options, httpPort, HttpProtocols.Http1);
                        Listen(options, grpcPort, HttpProtocols.Http2);
                    });
            });
            return builder;
        }
        static void Listen(KestrelServerOptions kestrelServerOptions, int? port, HttpProtocols protocols)
        {
            if (port == null)
                return;

            var address = IPAddress.Any;
            kestrelServerOptions.Listen(address, port.Value, listenOptions => { listenOptions.Protocols = protocols; });
        }
    }
}
