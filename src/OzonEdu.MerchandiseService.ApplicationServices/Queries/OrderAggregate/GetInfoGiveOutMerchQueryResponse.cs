using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using System;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class GetInfoGiveOutMerchQueryResponse
    {
        public DeliveryMerch[] DeliveryMerch { get; init; }
    }

    public class DeliveryMerch
    {
        public DateTimeOffset DeliveryDate { get; init; }
        public MerchPack MerchPack { get; init; }
    }
}