using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OzonEdu.MerchandiseService.Extensions;
using Serilog;
using WebApi.Infrastructure.Extensions;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
     => Host.CreateDefaultBuilder(args)
         .UseSerilog((context, configuration) => configuration
                .ReadFrom
                .Configuration(context.Configuration)
                .WriteTo.Console())
         .ConfigurePorts()
         .AddInfrastructure();
