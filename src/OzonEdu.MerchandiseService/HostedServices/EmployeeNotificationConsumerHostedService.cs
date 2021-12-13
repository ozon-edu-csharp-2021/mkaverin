using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Configuration;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.HostedServices
{
    public class EmployeeNotificationConsumerHostedService : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmployeeNotificationConsumerHostedService> _logger;

        protected string Topic { get; set; } = "employee_notification_event";

        public EmployeeNotificationConsumerHostedService(
            IOptions<KafkaConfiguration> config,
            IServiceScopeFactory scopeFactory,
            ILogger<EmployeeNotificationConsumerHostedService> logger)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _config.GroupId,
                BootstrapServers = _config.BootstrapServers,
            };

            using (var c = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                c.Subscribe(Topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            try
                            {
                                await Task.Yield();
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                var cr = c.Consume(stoppingToken);
                                if (cr != null)
                                {
                                    var message = JsonSerializer.Deserialize<NotificationEvent>(cr.Message.Value);
                                    if (message.Payload is JsonElement json)
                                    {
                                        var payload = JsonSerializer.Deserialize<MerchDeliveryEventPayload>(json.GetRawText());
                                        GiveOutNewOrderCommand giveOutNewOrderCommand = new()
                                        {
                                            EmployeeEmail = message.EmployeeEmail,
                                            EmployeeName = message.EmployeeName,
                                            ManagerEmail = message.ManagerEmail,
                                            ManagerName = message.ManagerName,
                                            EventType = (int)message.EventType,
                                            MerchType = (int)payload.MerchType,
                                            ClothingSize = (int)payload.ClothingSize,

                                        };
                                        giveOutNewOrderCommand.Source = 2;
                                        await mediator.Send(giveOutNewOrderCommand, stoppingToken);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Error while get consume. Message {ex.Message}");
                            }
                        }
                    }
                }
                finally
                {
                    c.Commit();
                    c.Close();
                }
            }
        }
    }
}