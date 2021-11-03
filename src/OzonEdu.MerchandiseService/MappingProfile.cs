using AutoMapper;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.GetInfoMerchResponseDto;
using System.Collections.Generic;

namespace EncashmentService.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            _ = CreateMap<GetInfoMerchRequestDto, GetInfoGiveOutMerchQuery>();
            _ = CreateMap<GetInfoGiveOutMerchQueryResponse, GetInfoMerchResponseDto>()
                .ForMember(e => e.DeliveryMerch, opt => opt.MapFrom(x => MappingArrayDeliveryMerch(x.DeliveryMerch)));
            _ = CreateMap<RequestMerchRequestDto, CreateOrderCommand>();
            _ = CreateMap<RequestMerchRequestDto, GiveOutOrderCommand>();

        }

        private MerchDelivery[] MappingArrayDeliveryMerch(DeliveryMerch[] deliveryMerch)
        {
            MerchDelivery[]? result = new MerchDelivery[deliveryMerch.Length];
            for (int i = 0; i < deliveryMerch.Length; i++)
            {
                result[i] = new()
                {
                    DeliveryDate = deliveryMerch[i].DeliveryDate,
                    MerchPack = new()
                    {
                        MerchType = deliveryMerch[i].MerchPack.MerchType.ToString(),
                        MerchItems = MappingListMerchItems(deliveryMerch[i].MerchPack.MerchItems)
                    }
                };
            }

            return result;
        }
        private List<Item> MappingListMerchItems(Dictionary<Sku, Quantity> merchItems)
        {
            List<Item> resultItems = new();
            foreach (KeyValuePair<Sku, Quantity> item in merchItems)
            {
                resultItems.Add(new Item()
                {
                    Sku = item.Key.Value,
                    Quantity = item.Value.Value
                });
            }
            return resultItems;
        }
    }
}
