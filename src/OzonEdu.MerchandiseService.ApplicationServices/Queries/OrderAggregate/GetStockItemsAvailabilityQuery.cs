using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.StockApi.Grpc;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class GetStockItemsAvailabilityQuery : IRequest<GiveOutItemsRequest>
    {
        public Dictionary<ItemTypeId, Quantity> MerchItems { get; set; }
        public ClothingSize Size { get; set; }
    }
}