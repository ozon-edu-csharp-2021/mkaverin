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

            _ = CreateMap<KeyValuePair<ItemTypeId, Quantity>, Item>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Key.Value))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Value.Value));

            _ = CreateMap<MerchPack, Pack>()
                .ForMember(dest => dest.MerchType, opt => opt.MapFrom(src => (int)src.MerchType)); ;
            _ = CreateMap<DeliveryMerch, MerchDelivery>();

            _ = CreateMap<GetInfoGiveOutMerchQueryResponse, GetInfoMerchResponseDto>();

            _ = CreateMap<RequestMerchRequestDto, GiveOutNewOrderCommand>();
            _ = CreateMap<RequestMerchRequestDto, GiveOutOrderCommand>();
            _ = CreateMap<bool, RequestMerchResponseDto>()
                .ForMember(e => e.Result, opt => opt.MapFrom(x => x));
        }
    }
}
