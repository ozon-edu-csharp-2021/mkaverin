using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
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

        private Dictionary<Sku, Quantity> SetMerchItems(string merchItemsJson)
        {
            var result = JsonSerializer.Deserialize<Dictionary<int, int>>(merchItemsJson);
            var merchItems = new Dictionary<Sku, Quantity>();
            foreach (var item in result)
            {
                merchItems.Add(new(item.Key), new(item.Value));
            }
            return merchItems;
        }

        public void AddToMerchItems(int skuId, int quantity)
        {
            Sku sku = new(skuId);
            var check = MerchItems.ContainsKey(sku);
            if (check)
            {
                var value = MerchItems[sku].Value;
                MerchItems[sku] = new(value += quantity);
            }
            else
            {
                MerchItems.Add(new(skuId), new(quantity));
            }
        }

        public void DeleteFromMerchItems(int skuId, int quantity)
        {
            Sku sku = new(skuId);
            var check = MerchItems.ContainsKey(sku);
            if (check)
            {
                var value = MerchItems[sku].Value;
                if (value > quantity)
                {
                    MerchItems[sku] = new(value - quantity);
                }
                else
                {
                    MerchItems.Remove(sku);
                }
            }
            else
            {
                throw new MerchTypeException($"Sku with ids {string.Join(", ", MerchItems.Keys)} not exists.");
            }
        }
    }
}