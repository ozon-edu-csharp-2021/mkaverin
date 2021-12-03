using Confluent.Kafka;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using OpenTracing;
using OzonEdu.MerchandiseService.ApplicationServices.MessageBroker;
using OzonEdu.MerchandiseService.Domain.Events;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.DomainEvent
{
    public class EmployeeNotificationMerchDeliveryEventHandler : INotificationHandler<EmployeeNotificationMerchDeliveryDomainEvent>
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;
        private readonly ITracer _tracer;

        public EmployeeNotificationMerchDeliveryEventHandler(IProducerBuilderWrapper producerBuilderWrapper, ITracer tracer)
        {
            _tracer = tracer;
            _producerBuilderWrapper = producerBuilderWrapper;
        }

        public Task Handle(EmployeeNotificationMerchDeliveryDomainEvent notification, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("EventHandler.EmployeeNotificationAboutSupply").StartActive();
            return _producerBuilderWrapper.Producer.ProduceAsync(_producerBuilderWrapper.EmployeeNotificationTopic,
                new Message<string, string>()
                {
                    Key = EmployeeEventType.MerchDelivery.ToString(),
                    Value = JsonSerializer.Serialize(new NotificationEvent()
                    {
                        EmployeeEmail = notification.EmployeeEmail.Value,
                        EmployeeName = notification.EmployeeName.Value,
                        ManagerEmail = notification.ManagerEmail.Value,
                        ManagerName = notification.ManagerName.Value,
                        EventType = EmployeeEventType.MerchDelivery,
                        Payload = new MerchDeliveryEventPayload()
                        {
                            MerchType = notification.MerchType,
                            ClothingSize = ClothingSize.XS
                           
                        }
                    })

                }, cancellationToken);
        }
    }
}