using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class EmployeeNotificationAboutSupplyDomainEvent : INotification
    {
        public Email EmployeeEmail { get; }
        public MerchPack MerchPack { get; }
        public EmployeeNotificationAboutSupplyDomainEvent(Email employeeEmail, MerchPack merchPack)
        {
            EmployeeEmail = employeeEmail;
            MerchPack = merchPack;
        }
    }
}
