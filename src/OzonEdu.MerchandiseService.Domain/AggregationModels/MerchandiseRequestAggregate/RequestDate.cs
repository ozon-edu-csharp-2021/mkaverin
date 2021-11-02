using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate
{
    public class RequestDate : ValueObject
    {
        public DateTimeOffset Value { get; }
        public RequestDate(DateTimeOffset date)
        {
            Value = date;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
