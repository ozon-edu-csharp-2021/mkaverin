using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{
    public class HRNotificationEndedMerchDamainEvent : INotification
    {
        public MerchPack MerchPack { get; }
        public HRNotificationEndedMerchDamainEvent(MerchPack merchPack)
        {
            MerchPack = merchPack;
        }
    }
}
