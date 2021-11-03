using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using OzonEdu.StockApi.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("v1/api/merch")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MerchandiseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RequestMerch")]
        public async Task<ActionResult<RequestMerchResponseDto>> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token)
        {
            bool checkMerchType = Enum.IsDefined(typeof(MerchType), requestDto.MerchType);
            if (!checkMerchType)
            {
                throw new ArgumentException(nameof(requestDto.MerchType));
            }

            CreateOrderCommand? createCommand = new()
            {
                MerchType = (MerchType)requestDto.MerchType,
                IdEmployee = requestDto.IdEmployee,
                Sourse = Source.External
            };
            int orderId = await _mediator.Send(createCommand, token);

            GiveOutOrderCommand? giveOutCommand = new()
            {
                OrderId = orderId
            };
            bool result = await _mediator.Send(giveOutCommand, token);

            return Ok(result);
        }

        [HttpGet("InfoMerch")]
        public async Task<ActionResult<GetInfoMerchResponseDto>> GetInfoMerch([FromQuery] GetInfoMerchRequestDto requestDto, CancellationToken token)
        {
            GetInformationIssuedMerchQuery? query = new()
            {
                EmployeeId = requestDto.IdEmployee
            };
            GetInformationIssuedMerchQueryResponse? result = await _mediator.Send(query, token);
            if (result.DeliveryMerch.Length == 0)
            {
                return NotFound();
            }
            #region Mapping response
            GetInfoMerchResponseDto response = new()
            {
                DeliveryMerch = new MerchDelivery[result.DeliveryMerch.Length]
            };
            for (int i = 0; i < result.DeliveryMerch.Length; i++)
            {
                response.DeliveryMerch[i] = new()
                {
                    DeliveryDate = result.DeliveryMerch[i].DeliveryDate,
                    MerchPack = new()
                    {
                        MerchType = result.DeliveryMerch[i].merchPack.MerchType.ToString(),
                        MerchItems = MappingListMerchItems(result.DeliveryMerch[i].merchPack.MerchItems)
                    }
                };
            }
            #endregion
            return Ok(response);
        }

        private List<Item> MappingListMerchItems(List<MerchItem> items)
        {
            List<Item>? resultItems = new();
            foreach (MerchItem? item in items)
            {
                resultItems.Add(new Item()
                {
                    Sku = item.Sku.Value,
                    Quantity = item.Quantity.Value
                });
            }
            return resultItems;
        }
    }
}
