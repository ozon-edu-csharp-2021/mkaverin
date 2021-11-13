using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class Status : Entity
    {
        public StatusType Type { get; }

        private static IEnumerable<StatusType> List() =>
             new[] { StatusType.New, StatusType.InQueue, StatusType.Notified, StatusType.Done };

        public Status(StatusType type)
        {
            var state = List().SingleOrDefault(s => s == type);

            if (state == null)
                throw new OrderStatusException($"Possible values for StatusType: {String.Join(",", List().Select(s => s.Name))}");

            Type = state;
        }

    }
}