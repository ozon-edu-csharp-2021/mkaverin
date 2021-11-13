using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public sealed class MerchPack : Entity
    {
        public MerchPack(MerchType merchType, Dictionary<Sku, Quantity> merchItems)
        {
            MerchType = merchType;
            MerchItems = merchItems;
        }
        public MerchType MerchType { get; private set; }
        public Dictionary<Sku, Quantity> MerchItems { get; private set; }
    }
}