using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class NewMerchandiseAppearedCommand : IRequest
    {
        public long SupplyId { get; set; }
        public Dictionary<Sku, Quantity> Items { get; set; }
    }
}