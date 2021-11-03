using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.OrderAggregate
{
    public class GetInformationIssuedMerchQueryHandler : IRequestHandler<GetInformationIssuedMerchQuery, GetInformationIssuedMerchQueryResponse>
    {
        public readonly IOrderRepository _orderRepository;

        public GetInformationIssuedMerchQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ??
                                         throw new ArgumentNullException($"{nameof(orderRepository)}");
        }

        public async Task<GetInformationIssuedMerchQueryResponse> Handle(GetInformationIssuedMerchQuery request, CancellationToken cancellationToken)
        {
            EmployeeId employeeIdRequest = new(request.EmployeeId);
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(employeeIdRequest);
            #region Mapping
            GetInformationIssuedMerchQueryResponse result = new()
            {
                DeliveryMerch = new DeliveryMerch[orders.Count]
            };
            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].Status == Status.Done)
                {
                    result.DeliveryMerch[i] = new DeliveryMerch()
                    {
                        DeliveryDate = orders[i].DeliveryDate.Value,
                        merchPack = orders[i].MerchPack
                    };
                }
            }
            #endregion
            return result;
        }
    }
}