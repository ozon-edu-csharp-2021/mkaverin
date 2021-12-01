using Confluent.Kafka;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using MediatR;
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

        public EmployeeNotificationMerchDeliveryEventHandler(IProducerBuilderWrapper producerBuilderWrapper)
        {
            _producerBuilderWrapper = producerBuilderWrapper;
        }

        public Task Handle(EmployeeNotificationMerchDeliveryDomainEvent notification, CancellationToken cancellationToken)
        {
            
            return _producerBuilderWrapper.Producer.ProduceAsync(_producerBuilderWrapper.EmployeeNotificationTopic,
                new Message<string, string>()
                {
                    Key = notification.MerchType.Id.ToString(),
                    Value = JsonSerializer.Serialize(new NotificationEvent()
                    {
                        EmployeeEmail = notification.EmployeeEmail.Value,
                        EmployeeName = notification.EmployeeName.Value,
                        ManagerEmail = notification.ManagerEmail.Value,
                        ManagerName = notification.ManagerName.Value,
                        EventType = EmployeeEventType.MerchDelivery,
                        Payload = new MerchDeliveryEventPayload()
                        {
                            MerchType = (MerchType)notification.MerchType.Id,
                            ClothingSize = ClothingSize.XS
                           
                        }
                    })

                }, cancellationToken);
        }
    }
}