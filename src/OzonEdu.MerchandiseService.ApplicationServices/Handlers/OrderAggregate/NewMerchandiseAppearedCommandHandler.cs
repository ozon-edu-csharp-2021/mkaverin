using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class NewMerchandiseAppearedCommandHandler : IRequestHandler<NewMerchandiseAppearedCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewMerchandiseAppearedCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException($"{nameof(unitOfWork)}");
        }

        public async Task<Unit> Handle(NewMerchandiseAppearedCommand request, CancellationToken cancellationToken)
        {
            var all = await _orderRepository.GetAllOrderInStatusAsync(new(StatusType.InQueue.Id), cancellationToken);
            all = all
                .Where(o => o.MerchPack.MerchItems.Any(sku => request.Items.Contains(sku)))
                .OrderBy(o => o.Source)
                .ToList();
            foreach (var item in all)
            {
                if (item.Source.Type.Equals(SourceType.External))
                {
                    bool isAvailable = GiveOutItems(item.MerchPack.MerchItems);
                    item.GiveOut(isAvailable, DateTimeOffset.UtcNow);
                    await _unitOfWork.StartTransaction(cancellationToken);
                    await _orderRepository.UpdateAsync(item, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                if (item.Source.Type.Equals(SourceType.Internal))
                {
                    bool isAvailable = GetStockItemsAvailability(item.MerchPack.MerchItems);
                    if (isAvailable)
                    {
                        item.ChangeStatusNotified();
                    }
                }
            }
            return Unit.Value;
        }

        private bool GetStockItemsAvailability(Dictionary<Sku, Quantity> merchItems)
        {
            return true;
        }

        // Обращаемся к сервису StockApi узнаем есть ли товар на складе
        private bool GiveOutItems(Dictionary<Sku, Quantity> merchItems)
        {
            return true;
        }
    }
}
