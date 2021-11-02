using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using System;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate
{
    public class GetInformationIssuedMerchQueryResponse
    {
        public DeliveryMerch[] DeliveryMerch { get; init; }
    }

    public class DeliveryMerch
    {
        public DateTimeOffset DeliveryDate { get; init; }
        public MerchPack merchPack { get; init; }
    }
}