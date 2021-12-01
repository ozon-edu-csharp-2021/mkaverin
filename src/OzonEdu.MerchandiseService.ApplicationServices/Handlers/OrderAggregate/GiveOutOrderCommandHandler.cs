using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.StockApi.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OzonEdu.StockApi.Grpc.GiveOutItemsResponse.Types;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class GiveOutOrderCommandHandler : IRequestHandler<GiveOutOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly StockApiGrpc.StockApiGrpcClient _stockApiGrpcClient;

        public GiveOutOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, StockApiGrpc.StockApiGrpcClient stockApiGrpcClient)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _stockApiGrpcClient = stockApiGrpcClient;
        }

        public async Task<bool> Handle(GiveOutOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.order?.Id is null or 0)
                throw new NoOrderException($"No order");
            var requestGiveOut = await GetByItemTypeAsync(request.order.MerchPack.MerchItems, request.order.ClothingSize, cancellationToken);
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

        private async Task<GiveOutItemsRequest> GetByItemTypeAsync(Dictionary<ItemTypeId, Quantity> merchItems, ClothingSizeType size, CancellationToken cancellationToken)
        {
            /*
                Не лучшее решение, так как по хорошему сервис StockApi должен нам вернуть (по типу мерча, размеру и кол-во) есть он или нет. 
                Но такого метода в ОЗОН не реализованно, а хранить у себя sku конкретных товаров это не корректно по бизнес логике.
             */
            var requestGiveOut = new GiveOutItemsRequest();
            foreach (var item in merchItems)
            {
                var requestGetByItemType = new IntIdModel() { Id = item.Key.Value };
                var itemsOfRequiredType = await _stockApiGrpcClient.GetByItemTypeAsync(requestGetByItemType, cancellationToken: cancellationToken);
                var itemOfRequiredSizeAndQuantity = itemsOfRequiredType.Items
                    .FirstOrDefault(i => (i.SizeId == null || i.SizeId == size.Id) && i.Quantity >= item.Value.Value);
                if (itemOfRequiredSizeAndQuantity is null)
                {
                    return null;
                }
                requestGiveOut.Items.Add(new SkuQuantityItem()
                {
                    Sku = itemOfRequiredSizeAndQuantity.Sku,
                    Quantity = itemOfRequiredSizeAndQuantity.Quantity
                });
            }
            return requestGiveOut;
        }
    }
}
