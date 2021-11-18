using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class CheckOrderExistsHandle : IRequestHandler<CheckOrderExistsQuery, long>
    {
        private readonly IOrderRepository _orderRepository;
        public CheckOrderExistsHandle(IOrderRepository orderRequestRepository)
        {
            _orderRepository = orderRequestRepository ?? throw new ArgumentNullException($"{nameof(orderRequestRepository)}");
        }

        public async Task<long> Handle(CheckOrderExistsQuery request, CancellationToken cancellationToken)
        {
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            Order order = orders
                .Where(r => r.MerchPack.Id == request.MerchType)
                .Where(r => r.Status.Id == StatusType.New.Id || r.Status.Id == StatusType.InQueue.Id)
                .FirstOrDefault();

            return order?.Id ?? -1;
        }
    }
}
