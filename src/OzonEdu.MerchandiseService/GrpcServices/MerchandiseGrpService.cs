using Grpc.Core;
using MediatR;
using OzonEdu.MerchandiseService.Grpc;
using System;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.GrpcServices
{
    public class MerchandiseGrpService : MerchandiseGrpc.MerchandiseGrpcBase
    {
        private readonly IMediator _mediator;
        public MerchandiseGrpService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task<GetInfoMerchResponse> GetInfoMerch(GetInfoMerchRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
            //var response = await _mediator.Send(new GetAllStockItemsQuery(), context.CancellationToken);
            //Тут общение с кафкой. Пока не проходили
        }

        public override Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
            //var response = await _mediator.Send(new GetAllStockItemsQuery(), context.CancellationToken);
            //Тут общение с кафкой. Пока не проходили
        }
    }
}
