using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {

        public readonly IOrderRepository _orderRepository;
        public readonly IMerchPackRepository _merchPackRepository;
        private readonly IMediator _mediator;
        public CreateOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IMerchPackRepository merchPackRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _merchPackRepository = merchPackRepository ?? throw new ArgumentNullException($"{nameof(merchPackRepository)}");
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            CheckGiveOutMerchByEmployeeIdQuery query = new()
            {
                EmployeeId = request.EmployeeId,
                MerchType = request.MerchType
            };
            bool checkGiveOut = await _mediator.Send(query, cancellationToken);
            if (checkGiveOut)
            {
                throw new MerchAlreadyGiveOutException("The employee has already been issued merch");
            }

            MerchPack merchPack = await _merchPackRepository.FindByTypeAsync(request.MerchType);
            Order requestMR = new(date: new(DateTimeOffset.UtcNow),
                                                employeeId: new(request.EmployeeId),
                                                merchPack: merchPack,
                                                source: request.Sourse);

            Order createdOrder = await _orderRepository.CreateAsync(requestMR, cancellationToken);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return createdOrder.Id;
        }
    }
}
