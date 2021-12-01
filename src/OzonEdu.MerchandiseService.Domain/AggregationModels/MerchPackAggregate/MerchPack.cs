using CSharpCourse.Core.Lib.Enums;
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
            MerchType = (MerchType)merch_type_id;
            MerchItems = SetMerchItems(merch_items);
        }
        public MerchType MerchType { get; private set; }
        public Dictionary<ItemTypeId, Quantity> MerchItems { get; private set; }

        private Dictionary<ItemTypeId, Quantity> SetMerchItems(string merchItemsJson)
        {
            var result = JsonSerializer.Deserialize<Dictionary<int, int>>(merchItemsJson);
            var merchItems = new Dictionary<ItemTypeId, Quantity>();
            foreach (var item in result)
            {
                merchItems.Add(new(item.Key), new(item.Value));
            }
            return merchItems;
        }

        public void AddToMerchItems(int itemTypeId, int quantity)
        {
            ItemTypeId sku = new(itemTypeId);
            var check = MerchItems.ContainsKey(sku);
            if (check)
            {
                var value = MerchItems[sku].Value;
                MerchItems[sku] = new(value += quantity);
            }
            else
            {
                MerchItems.Add(new(itemTypeId), new(quantity));
            }
        }

        public void DeleteFromMerchItems(int itemTypeId, int quantity)
        {
            ItemTypeId item = new(itemTypeId);
            var check = MerchItems.ContainsKey(item);
            if (check)
            {
                var value = MerchItems[item].Value;
                if (value > quantity)
                {
                    MerchItems[item] = new(value - quantity);
                }
                else
                {
                    MerchItems.Remove(item);
                }
            }
            else
            {
                throw new MerchTypeException($"Sku with ids {string.Join(", ", MerchItems.Keys)} not exists.");
            }
        }
    }
}