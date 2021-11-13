using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class Quantity : ValueObject
    {
        public int Value { get; }

        public Quantity(int value)
        {
            if (value <= 0)
                throw new ArgumentException(nameof(Quantity));
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}