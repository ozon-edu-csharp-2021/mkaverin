using Grpc.Net.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OzonEdu.MerchandiseService.ApplicationServices.Configuration;
using OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate;
using OzonEdu.MerchandiseService.ApplicationServices.HttpClients;
using OzonEdu.MerchandiseService.ApplicationServices.MessageBroker;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Implementation;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.HostedServices;
using OzonEdu.StockApi.Grpc;
using System;

namespace OzonEdu.MerchandiseService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GiveOutNewOrderCommandHandler).Assembly);
            services.AddAutoMapper(typeof(Startup));
            return services;
        }
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<StockReplenishedConsumerHostedService>();
            services.AddHostedService<EmployeeNotificationConsumerHostedService>();

            return services;
        }
        public static IServiceCollection AddDatabaseConnection(this IServiceCollection services,
          IConfiguration configuration)
        {
            services.Configure<DatabaseConnectionOptions>(configuration.GetSection(nameof(DatabaseConnectionOptions)));

            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();

            return services;
        }
        public static IServiceCollection AddKafkaServices(this IServiceCollection services,
          IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            services.AddScoped<IProducerBuilderWrapper, ProducerBuilderWrapper>();

            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMerchPackRepository, MerchPackRepository>();
            return services;
        }
        public static IServiceCollection AddExternalServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddStockGrpcServiceClient(configuration);
            services.AddEmployeesHttpServiceClient(configuration);
            return services;
        }

        public static IServiceCollection AddStockGrpcServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionAddress = configuration.GetSection(nameof(StockApiGrpcServiceConfiguration))
                .Get<StockApiGrpcServiceConfiguration>().ServerAddress;
            if (string.IsNullOrWhiteSpace(connectionAddress))
                connectionAddress = configuration
                    .Get<StockApiGrpcServiceConfiguration>()
                    .ServerAddress;

            services.AddScoped<StockApiGrpc.StockApiGrpcClient>(opt =>
            {
                var channel = GrpcChannel.ForAddress(connectionAddress);
                return new StockApiGrpc.StockApiGrpcClient(channel);
            });

            return services;
        }
        public static IServiceCollection AddEmployeesHttpServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionAddress = configuration.GetSection(nameof(EmployeesHttpServiceConfiguration))
                .Get<EmployeesHttpServiceConfiguration>().ServerAddress;
            if (string.IsNullOrWhiteSpace(connectionAddress))
                connectionAddress = configuration
                    .Get<EmployeesHttpServiceConfiguration>()
                    .ServerAddress;
            services.AddHttpClient<IEmployeesHttpClient, EmployeesHttpClient>(client => client.BaseAddress = new Uri(connectionAddress));
            return services;
        }
    }
}
