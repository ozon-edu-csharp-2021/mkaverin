using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.GetInfoMerchResponseDto;
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
        private readonly ITracer _tracer;
        public MerchandiseController(IMediator mediator, IMapper mapper, ITracer tracer)
        {
            _mediator = mediator;
            _mapper = mapper;
            _tracer = tracer;
        }

        [HttpPost("RequestMerch")]
        public async Task<ActionResult<RequestMerchResponseDto>> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("HttpPost.RequestMerch").StartActive();
            GiveOutNewOrderCommand giveOutNewOrderCommand = _mapper.Map<GiveOutNewOrderCommand>(requestDto);
            giveOutNewOrderCommand.Source = 1;
            var result = await _mediator.Send(giveOutNewOrderCommand, cancellationToken);
            RequestMerchResponseDto response = _mapper.Map<RequestMerchResponseDto>(result);
            return Ok(response);
        }

        [HttpGet("InfoMerch")]
        public async Task<ActionResult<GetInfoMerchResponseDto>> GetInfoMerch([FromQuery] GetInfoMerchRequestDto requestDto, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("HttpGet.InfoMerch").StartActive();
            GetInfoGiveOutMerchQuery query = _mapper.Map<GetInfoGiveOutMerchQuery>(requestDto);
            GetInfoGiveOutMerchQueryResponse result = await _mediator.Send(query, cancellationToken);
            if (result.DeliveryMerch.Length == 0)
                return NotFound();
            GetInfoMerchResponseDto response = _mapper.Map<GetInfoMerchResponseDto>(result); 
            return Ok(response);
        }
    }
}
