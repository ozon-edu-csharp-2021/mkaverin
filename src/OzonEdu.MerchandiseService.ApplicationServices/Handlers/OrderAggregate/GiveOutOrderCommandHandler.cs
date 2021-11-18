using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class GiveOutOrderCommandHandler : IRequestHandler<GiveOutOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public GiveOutOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)}");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException($"{nameof(unitOfWork)}");
        }

        public async Task<bool> Handle(GiveOutOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
            {
                throw new NoOrderException($"No order with id {request.OrderId}");
            }

            CheckGiveOutMerchByEmployeeIdQuery query = new()
            {
                EmployeeId = order.EmployeeId.Value,
                MerchType = order.MerchPack.MerchType.Type.Id
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
                await _orderRepository.UpdateAsync(order, cancellationToken);
                return true;
            }
            else
            {
                order.ChangeStatusToInQueue();
                await _orderRepository.UpdateAsync(order, cancellationToken);
                return false;
            }
            
        }
    }
}
