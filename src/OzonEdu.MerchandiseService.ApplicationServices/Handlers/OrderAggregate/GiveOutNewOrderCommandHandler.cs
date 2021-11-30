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
    public class GiveOutNewOrderCommandHandler : IRequestHandler<GiveOutNewOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public GiveOutNewOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _merchPackRepository = merchPackRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;

        }

        public async Task<bool> Handle(GiveOutNewOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);
            MerchPack merchPack = await _merchPackRepository.FindByTypeAsync(new(request.MerchType), cancellationToken);
            IReadOnlyCollection<Order> orders = await _orderRepository.GetAllOrderByEmployeeAsync(request.EmployeeEmail, cancellationToken);
            Order requestMR = Order.Create(date: new(DateTimeOffset.UtcNow),
                                                employeeEmail: Email.Crate(request.EmployeeEmail),
                                                managerEmail: Email.Crate(request.ManagerEmail),
                                                merchPack: merchPack,
                                                source: new(request.Source), orders);

            var createdOrderId = await _orderRepository.CreateAsync(requestMR, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = await _mediator.Send(new GiveOutOrderCommand { order = new(createdOrderId, requestMR) }, cancellationToken);
            return result;
        }
    }
}
