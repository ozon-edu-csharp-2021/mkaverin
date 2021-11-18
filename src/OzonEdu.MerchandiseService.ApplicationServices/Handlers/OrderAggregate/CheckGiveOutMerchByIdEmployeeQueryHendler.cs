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
    public class CheckGiveOutMerchByEmployeeIdQueryHendler : IRequestHandler<CheckGiveOutMerchByEmployeeIdQuery, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckGiveOutMerchByEmployeeIdQueryHendler(IOrderRepository orderRequestRepository)
        {
            _orderRepository = orderRequestRepository ?? throw new ArgumentNullException($"{nameof(orderRequestRepository)}");
        }

        public async Task<bool> Handle(CheckGiveOutMerchByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(request.EmployeeId);
            List<Order> checkGiveOut = orders
                .Where(r => r.Status.Id == StatusType.Done.Id)
                .Where(r => IsYearPassedBetweenDates(r.DeliveryDate.Value, DateTimeOffset.UtcNow.Date))
                .Where(r => r.MerchPack.MerchType.Id == request.MerchType).ToList();

            return checkGiveOut.Any();
        }

        private bool IsYearPassedBetweenDates(DateTimeOffset deliveryDate, DateTimeOffset today)
        {
            var betweenYear = today.Year - deliveryDate.Year;
            return today.AddYears(-betweenYear).Date >= deliveryDate.Date;
        }
    }
}
