using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class GiveOutOrderCommandHandler : IRequestHandler<GiveOutOrderCommand, bool>
    {
        public readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public GiveOutOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _mediator = mediator;
        }

        public async Task<bool> Handle(GiveOutOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = await _orderRepository.FindByIdAsync(request.OrderId);
            if (order is not null)
            {
                throw new NoOrderException($"No order with id {request.OrderId}");
            }

            CheckGiveOutMerchByEmployeeIdQuery query = new()
            {
                EmployeeId = order.EmployeeId.Value,
                MerchType = order.MerchPack.MerchType
            };
            bool checkGiveOut = await _mediator.Send(query, cancellationToken);
            if (checkGiveOut)
            {
                throw new MerchAlreadyGiveOutException("The employee has already been issued merch");
            }

            // Обращаемся к сервису StockApi узнаем есть ли товар на складе
            if (true)
            {
                order.ChangeStatusToDone(DateTimeOffset.UtcNow);
                return true;
            }
            else
            {
                order.ChangeStatusToInQueue();
                return false;
            }
        }
    }
}
