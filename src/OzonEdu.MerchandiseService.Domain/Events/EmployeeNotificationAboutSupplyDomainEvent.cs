using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class EmployeeNotificationAboutSupplyDomainEvent : INotification
    {
        public EmployeeId EmployeeId { get; }

        public EmployeeNotificationAboutSupplyDomainEvent(EmployeeId employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
