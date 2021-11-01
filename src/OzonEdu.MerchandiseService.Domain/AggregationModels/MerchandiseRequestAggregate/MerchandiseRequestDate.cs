using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate
{
    public class MerchandiseRequestDate : ValueObject
    {
        public DateTimeOffset Value { get; }
        public MerchandiseRequestDate(DateTimeOffset date)
        {
            Value = date;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
