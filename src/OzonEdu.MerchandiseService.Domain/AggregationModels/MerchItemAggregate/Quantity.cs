using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class Quantity : ValueObject
    {
        public int Value { get; }

        public Quantity(int value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}