using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public sealed record EmployeeNotificationMerchDeliveryDomainEvent : INotification
    {
        public Email EmployeeEmail { get; set; }
        public NameUser EmployeeName { get; set; }
        public Email ManagerEmail { get; set; }
        public NameUser ManagerName { get; set; }
        public MerchType MerchType { get; init; }
    }
}
