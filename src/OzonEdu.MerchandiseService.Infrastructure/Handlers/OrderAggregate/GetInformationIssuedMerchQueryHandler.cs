using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.OrderAggregate
{
    public class GetInformationIssuedMerchQueryHandler : IRequestHandler<GetInfoGiveOutMerchQuery, GetInfoGiveOutMerchQueryResponse>
    {
        public readonly IOrderRepository _orderRepository;

        public GetInformationIssuedMerchQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
        }

        public async Task<GetInfoGiveOutMerchQueryResponse> Handle(GetInfoGiveOutMerchQuery request, CancellationToken cancellationToken)
        {
            EmployeeId employeeIdRequest = new(request.EmployeeId);
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(employeeIdRequest);
            #region Mapping result
            GetInfoGiveOutMerchQueryResponse result = new()
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
                        MerchPack = orders[i].MerchPack
                    };
                }
            }
            #endregion
            return result;
        }
    }
}