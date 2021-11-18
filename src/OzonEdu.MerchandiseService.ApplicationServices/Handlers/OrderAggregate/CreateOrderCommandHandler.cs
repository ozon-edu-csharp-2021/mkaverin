using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _merchPackRepository = merchPackRepository ?? throw new ArgumentNullException($"{nameof(merchPackRepository)}");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException($"{nameof(unitOfWork)}");
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
            IReadOnlyCollection<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            MerchPack merchPack = await _merchPackRepository.FindByTypeAsync(new(request.MerchType), cancellationToken);
            Order requestMR = Order.Create(date: new(DateTimeOffset.UtcNow),
                                                employeeId: new(request.EmployeeId),
                                                merchPack: merchPack,
                                                source: new(request.Sourse), orders);

            var createdOrderId = await _orderRepository.CreateAsync(requestMR, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return createdOrderId;
        }
    }
}
