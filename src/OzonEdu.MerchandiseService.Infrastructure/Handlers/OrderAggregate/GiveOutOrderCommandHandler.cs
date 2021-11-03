using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Queries.OrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.OrderAggregate
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

            CheckGiveOutMerchByIdEmployeeQuery query = new()
            {
                EmployeeId = order.EmployeeId.Value,
                MerchType = order.MerchPack.MerchType
            };
            bool checkGiveOut = await _mediator.Send(query, cancellationToken);
            if (checkGiveOut)
            {
                throw new MerchAlreadyGiveOutException("The employee has already been issued merch");
            }

            //Обращаемся к сервису StockApi узнаем есть ли товар на складе
            //Если нету то вызываем 
            order.ChangeStatusToInQueue();
            return false;

            //Если есть 
            //Обращаемся к сервису StockApi и резервируем товар на складе
            //вызываем 
            order.ChangeStatusToDone(DateTimeOffset.UtcNow);
            return true;
        }
    }
}
