using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class EmployeeNotificationAboutSupplyDamainEvent : INotification
    {
        public EmployeeId EmployeeId { get; }

        public EmployeeNotificationAboutSupplyDamainEvent(EmployeeId employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
