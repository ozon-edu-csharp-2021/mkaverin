using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.GetInfoMerchResponseDto
{
    public class Pack
    {
        public string MerchType { get; init; }
        public List<Item> MerchItems { get; init; }
    }
}
