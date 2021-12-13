using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class ItemTypeId : ValueObject
    {
        public int Value { get; }

        public ItemTypeId(int id)
        {
            if (id <= 0)
                throw new ArgumentException(nameof(ItemTypeId));
            Value = id;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        public override string ToString() => Value.ToString();
    }
}