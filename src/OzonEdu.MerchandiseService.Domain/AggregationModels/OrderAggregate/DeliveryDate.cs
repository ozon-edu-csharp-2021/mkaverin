using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class DeliveryDate : ValueObject
    {
        public DateTimeOffset Value { get; }
        private DeliveryDate(DateTimeOffset date) => Value = date;
        public static DeliveryDate Create(DateTimeOffset? date)
            => date is null ? null : new DeliveryDate(date ?? DateTimeOffset.MinValue);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
