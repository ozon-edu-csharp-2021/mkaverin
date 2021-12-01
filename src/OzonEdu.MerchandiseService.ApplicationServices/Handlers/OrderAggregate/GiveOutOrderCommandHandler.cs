using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.StockApi.Grpc;
using System;
using System.Threading;
using System.Threading.Tasks;
using static OzonEdu.StockApi.Grpc.GiveOutItemsResponse.Types;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class GiveOutOrderCommandHandler : IRequestHandler<GiveOutOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly StockApiGrpc.StockApiGrpcClient _stockApiGrpcClient;

        public GiveOutOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IUnitOfWork unitOfWork, StockApiGrpc.StockApiGrpcClient stockApiGrpcClient)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _stockApiGrpcClient = stockApiGrpcClient;
            _mediator = mediator;
        }

        public async Task<bool> Handle(GiveOutOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.order?.Id is null or 0)
                throw new NoOrderException($"No order");

            var requestGiveOut = await _mediator.Send(new GetStockItemsAvailabilityQuery { MerchItems = request.order.MerchPack.MerchItems, Size = request.order.ClothingSize }, cancellationToken);
            bool isAvailable = await GiveOutItemsAsync(requestGiveOut, cancellationToken);
            request.order.GiveOut(isAvailable, DateTimeOffset.UtcNow);
            await _orderRepository.UpdateAsync(request.order, cancellationToken);
            return isAvailable;
        }

        private async Task<bool> GiveOutItemsAsync(GiveOutItemsRequest requestGiveOut, CancellationToken cancellationToken)
        {
            if (requestGiveOut is null)
            {
                return false;
            }
            var resultGiveOut = await _stockApiGrpcClient.GiveOutItemsAsync(requestGiveOut, cancellationToken: cancellationToken);
            return resultGiveOut.Result == Result.Successful;
        }
    }
}
