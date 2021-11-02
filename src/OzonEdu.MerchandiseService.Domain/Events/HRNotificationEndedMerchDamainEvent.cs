using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

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
