using System;

namespace OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.GetInfoMerchResponseDto
{
    public class MerchDelivery
    {
        public DateTimeOffset DeliveryDate { get; init; }
        public Pack MerchPack { get; init; }
    }
}
