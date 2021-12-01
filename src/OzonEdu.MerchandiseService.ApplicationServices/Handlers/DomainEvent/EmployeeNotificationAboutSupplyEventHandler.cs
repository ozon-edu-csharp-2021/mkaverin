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
    public class EmployeeNotificationAboutSupplyEventHandler : INotificationHandler<EmployeeNotificationAboutSupplyDomainEvent>
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;

        public EmployeeNotificationAboutSupplyEventHandler(IProducerBuilderWrapper producerBuilderWrapper)
        {
            _producerBuilderWrapper = producerBuilderWrapper;
        }

        public Task Handle(EmployeeNotificationAboutSupplyDomainEvent notification, CancellationToken cancellationToken)
        {
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
                         EventType = EmployeeEventType.MerchDelivery, //Тут нужен другой тип, но его пока нет на стороне инфраструктуры Озон
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