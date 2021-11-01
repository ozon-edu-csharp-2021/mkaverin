using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public class MerchPack : Entity
    {
        public MerchPack(MerchType merchType, List<MerchItem> merchItems)
        {
            MerchType = merchType;
            MerchItems = merchItems;
        }
        public MerchType MerchType { get; private set; }
        public List<MerchItem> MerchItems { get; }
    }
}