using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class Source : Entity
    {
        public SourceType Type { get; }

        private static IEnumerable<SourceType> List() =>
            new[] { SourceType.External, SourceType.Internal };

        public Source(long typeId)
        {
            var state = List().SingleOrDefault(s => s.Id == typeId);

            if (state == null)
                throw new OrderStatusException($"Possible values for SourceType: {String.Join(",", List().Select(s => s.Name))}");

            Type = state;
            Id = Type.Id;
        }
    }
}