using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.Exceptions;
using OzonEdu.StockApi.Infrastructure.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.OrderAggregate
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {

        public readonly IOrderRepository _orderRepository;
        public readonly IMerchPackRepository _merchPackRepository;
        private readonly IMediator _mediator;
        public CreateOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IMerchPackRepository merchPackRepository)
        {
            _orderRepository = orderRepository ??
                                         throw new ArgumentNullException($"{nameof(orderRepository)}");
            _merchPackRepository = merchPackRepository ??
                                       throw new ArgumentNullException($"{nameof(merchPackRepository)}");
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            CheckGiveOutMerchByIdEmployeeQuery query = new()
            {
                EmployeeId = request.IdEmployee,
                MerchType = request.MerchType
            };
            bool checkGiveOut = await _mediator.Send(query, cancellationToken);
            if (checkGiveOut)
            {
                throw new MerchAlreadyGiveOutException("The employee has already been issued merch");
            }

            MerchPack merchPack = await _merchPackRepository.FindByTypeAsync(request.MerchType);
            Order requestMR = new(date: new(DateTimeOffset.UtcNow),
                                                idEmployee: new(request.IdEmployee),
                                                merchPack: merchPack,
                                                source: request.Sourse);

            Order createdOrder = await _orderRepository.CreateAsync(requestMR, cancellationToken);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return createdOrder.Id;
        }
    }
}
