using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using CSharpCourse.Core.Lib.Models;
using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.MessageBroker;
using OzonEdu.MerchandiseService.Domain.Events;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.DomainEvent
{
    public class HRNotificationMerchEndedEventHandler : INotificationHandler<HRNotificationMerchEndedDomainEvent>
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;

        public HRNotificationMerchEndedEventHandler(IProducerBuilderWrapper producerBuilderWrapper)
        {
            _producerBuilderWrapper = producerBuilderWrapper;
        }

        public Task Handle(HRNotificationMerchEndedDomainEvent notification, CancellationToken cancellationToken)
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
                        EventType = EmployeeEventType.MerchDelivery, //Тут нужен другой тип, но его пока нет на стороне инфраструктуры Озон
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