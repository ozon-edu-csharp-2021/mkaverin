
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchPack : Entity
    {
        public MerchPack(long id, int merch_type_id, string merch_items)
        {
            Id = id;
            MerchType = new(merch_type_id);
            MerchItems = SetMerchItems(merch_items);
        }
        public MerchType MerchType { get; private set; }
        public Dictionary<Sku, Quantity> MerchItems { get; private set; }

        Dictionary<Sku, Quantity> SetMerchItems(string merchItemsJson)
        {
            var result = JsonSerializer.Deserialize<Dictionary<Sku, Quantity>>(merchItemsJson);
            return result;
        }
    }
}