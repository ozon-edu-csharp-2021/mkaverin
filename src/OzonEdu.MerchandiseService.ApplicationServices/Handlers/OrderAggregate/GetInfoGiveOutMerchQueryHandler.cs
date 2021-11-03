using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
            EmployeeId employeeIdRequest = new(request.EmployeeId);
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(employeeIdRequest);
            var ordersDone = orders.Where(o => o.Status.Equals(Status.Done)).ToList();
            #region Mapping result
            GetInfoGiveOutMerchQueryResponse result = new()
            {
                DeliveryMerch = new DeliveryMerch[ordersDone.Count]
            };
            for (int i = 0; i < ordersDone.Count; i++)
            {
                if (ordersDone[i].Status == Status.Done)
                {
                    result.DeliveryMerch[i] = new DeliveryMerch()
                    {
                        DeliveryDate = ordersDone[i].DeliveryDate.Value,
                        MerchPack = ordersDone[i].MerchPack
                    };
                }
            }
            #endregion
            return result;
        }
    }
}