using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public sealed class EmployeeId : ValueObject
    {
        public long Value { get; }
        public EmployeeId(long id)
        {
            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
