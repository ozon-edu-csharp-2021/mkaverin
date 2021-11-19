using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class GiveOutOrderCommandHandler : IRequestHandler<GiveOutOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GiveOutOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException($"{nameof(unitOfWork)}");
        }

        public async Task<bool> Handle(GiveOutOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.order is null || request.order.Id == 0)
                throw new NoOrderException($"No order");
            bool isAvailable = GiveOutItems(request.order.MerchPack.MerchItems);

            request.order.GiveOut(isAvailable, DateTimeOffset.UtcNow);
            await _orderRepository.UpdateAsync(request.order, cancellationToken);
            return isAvailable;
        }

        // Обращаемся к сервису StockApi узнаем есть ли товар на складе
        private bool GiveOutItems(Dictionary<Sku, Quantity> merchItems)
        {
            return true;
        }
    }
}
