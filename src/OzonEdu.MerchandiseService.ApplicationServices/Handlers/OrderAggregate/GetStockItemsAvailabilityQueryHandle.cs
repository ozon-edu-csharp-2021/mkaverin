using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.StockApi.Grpc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class GetStockItemsAvailabilityQueryHandle : IRequestHandler<GetStockItemsAvailabilityQuery, GiveOutItemsRequest>
    {
        private readonly StockApiGrpc.StockApiGrpcClient _stockApiGrpcClient;

        public GetStockItemsAvailabilityQueryHandle(StockApiGrpc.StockApiGrpcClient stockApiGrpcClient)
        {
            _stockApiGrpcClient = stockApiGrpcClient;
        }

        public async Task<GiveOutItemsRequest> Handle(GetStockItemsAvailabilityQuery request, CancellationToken cancellationToken)
        {
            /*
                Не лучшее решение, так как по хорошему сервис StockApi должен нам вернуть (по типу мерча, размеру и кол-во) есть он или нет. 
                Но такого метода в ОЗОН не реализованно, а хранить у себя sku конкретных товаров это не корректно по бизнес логике.
             */
            var requestGiveOut = new GiveOutItemsRequest();
            foreach (var item in request.MerchItems)
            {
                var requestGetByItemType = new IntIdModel() { Id = item.Key.Value };
                var itemsOfRequiredType = await _stockApiGrpcClient.GetByItemTypeAsync(requestGetByItemType, cancellationToken: cancellationToken);
                var itemOfRequiredSizeAndQuantity = itemsOfRequiredType.Items
                    .FirstOrDefault(i => (i.SizeId == null || i.SizeId == (int)request.Size) && i.Quantity >= item.Value.Value);
                if (itemOfRequiredSizeAndQuantity is null)
                {
                    return null;
                }
                requestGiveOut.Items.Add(new SkuQuantityItem()
                {
                    Sku = itemOfRequiredSizeAndQuantity.Sku,
                    Quantity = item.Value.Value
                });
            }
            return requestGiveOut;
        }
    }
}
