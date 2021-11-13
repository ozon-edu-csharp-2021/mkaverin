using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class HRNotificationEndedMerchDomainEvent : INotification
    {
        public MerchPack MerchPack { get; }
        public HRNotificationEndedMerchDomainEvent(MerchPack merchPack)
        {
            MerchPack = merchPack;
        }
    }
}
