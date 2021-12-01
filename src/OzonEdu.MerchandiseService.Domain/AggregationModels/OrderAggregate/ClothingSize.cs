using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class ClothingSize : Entity
    {
        public ClothingSizeType Type { get; }

        private static IEnumerable<ClothingSizeType> List() =>
             new[] {
                 ClothingSizeType.XS,
                 ClothingSizeType.S,
                 ClothingSizeType.M,
                 ClothingSizeType.L,
                 ClothingSizeType.XL,
                 ClothingSizeType.XXL
             };

        public ClothingSize(long id)
        {
            var size = List().SingleOrDefault(s => s.Id == id);

            if (size == null)
                throw new OrderStatusException($"Possible values for ClothingSizeType: {String.Join(",", List().Select(s => s.Name))}");

            Type = size;
            Id = Type.Id;
        }
        public ClothingSize(string name)
        {
            var size = List().SingleOrDefault(s => s.Name == name.ToUpperInvariant());

            if (size == null)
                throw new OrderStatusException($"Possible values for ClothingSizeType: {String.Join(",", List().Select(s => s.Name))}");

            Type = size;
            Id = Type.Id;
        }
    }
}
