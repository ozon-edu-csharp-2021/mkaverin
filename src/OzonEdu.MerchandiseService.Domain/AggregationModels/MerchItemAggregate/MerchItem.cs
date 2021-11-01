using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class MerchItem : Entity
    {
        public MerchItem(Sku sku, Quantity quantity)
        {
            Sku = sku;
            Quantity = quantity;
        }
        public Sku Sku { get; private set; }
        public Quantity Quantity { get; private set; }
    }
}