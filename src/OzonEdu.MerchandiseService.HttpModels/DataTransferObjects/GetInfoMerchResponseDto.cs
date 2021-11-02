using System;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.HttpModels.DataTransferObjects
{
    public class GetInfoMerchResponseDto
    {
        public MerchDelivery[] DeliveryMerch { get; init; }
    }

    public class MerchDelivery
    {
        public DateTimeOffset DeliveryDate { get; init; }
        public Pack MerchPack { get; init; }
    }
    public class Pack
    {
        public string MerchType { get; init; }
        public List<Item> MerchItems { get; init; }
    }

    public class Item
    {
        public long Sku { get; init; }
        public int Quantity { get; init; }
    }
}
