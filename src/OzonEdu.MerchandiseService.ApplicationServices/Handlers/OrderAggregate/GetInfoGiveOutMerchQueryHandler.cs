using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class GetInfoGiveOutMerchQueryHandler : IRequestHandler<GetInfoGiveOutMerchQuery, GetInfoGiveOutMerchQueryResponse>
    {
        public readonly IOrderRepository _orderRepository;

        public GetInfoGiveOutMerchQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
        }

        public async Task<GetInfoGiveOutMerchQueryResponse> Handle(GetInfoGiveOutMerchQuery request, CancellationToken cancellationToken)
        {
            List<Order> ordersDone = await _orderRepository.GetAllOrderByEmployeeIdAsync(request.EmployeeId, new Status(StatusType.Done));
            #region Mapping result
            GetInfoGiveOutMerchQueryResponse result = new()
            {
                DeliveryMerch = new DeliveryMerch[ordersDone.Count]
            };
            for (var i = 0; i < ordersDone.Count; i++)
            {
                result.DeliveryMerch[i] = new DeliveryMerch()
                {
                    DeliveryDate = ordersDone[i].DeliveryDate.Value,
                    MerchPack = ordersDone[i].MerchPack
                };
            }
            #endregion
            return result;
        }
    }
}