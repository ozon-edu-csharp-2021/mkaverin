using AutoMapper;
using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.GetInfoMerchResponseDto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("v1/api/merch")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public MerchandiseController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("RequestMerch")]
        public async Task<ActionResult<RequestMerchResponseDto>> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token)
        {
            if (!Enum.IsDefined(typeof(MerchType), requestDto.MerchType))
                throw new ArgumentException(nameof(requestDto.MerchType));

            CreateOrderCommand? createCommand = _mapper.Map<CreateOrderCommand>(requestDto);
            createCommand.Sourse = Source.External;

            int orderId = await _mediator.Send(createCommand, token);

            bool result = await _mediator.Send(new GiveOutOrderCommand(orderId), token);

            return Ok(result);
        }

        [HttpGet("InfoMerch")]
        public async Task<ActionResult<GetInfoMerchResponseDto>> GetInfoMerch([FromQuery] GetInfoMerchRequestDto requestDto, CancellationToken token)
        {
            GetInfoGiveOutMerchQuery? query = _mapper.Map<GetInfoGiveOutMerchQuery>(requestDto);
            GetInfoGiveOutMerchQueryResponse? result = await _mediator.Send(query, token);
            if (result.DeliveryMerch.Length == 0)
                return NotFound();

            GetInfoMerchResponseDto? response = _mapper.Map<GetInfoMerchResponseDto>(result);
            return Ok(response);
        }
    }
}
