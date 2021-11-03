using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.OrderAggregate
{
    public class CheckGiveOutMerchByIdEmployeeQueryHendler : IRequestHandler<CheckGiveOutMerchByIdEmployeeQuery, bool>
    {
        private const int DAYSYEAR = 366;

        public readonly IOrderRepository _orderRepository;

        public CheckGiveOutMerchByIdEmployeeQueryHendler(IOrderRepository orderRequestRepository)
        {
            _orderRepository = orderRequestRepository ??
                                         throw new ArgumentNullException($"{nameof(orderRequestRepository)}");
        }

        public async Task<bool> Handle(CheckGiveOutMerchByIdEmployeeQuery request, CancellationToken cancellationToken)
        {
            EmployeeId employeeIdRequest = new(request.EmployeeId);
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(employeeIdRequest);

            IEnumerable<Order> checkGiveOut = orders
                .Where(r => (DateTimeOffset.UtcNow - r.DeliveryDate.Value).TotalDays < DAYSYEAR)
                .Where(r => r.MerchPack.MerchType == request.MerchType && r.Status == Status.Done);

            return checkGiveOut.Any();
        }
    }
}
