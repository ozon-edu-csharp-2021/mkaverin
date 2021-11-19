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
        private readonly IMediator _mediator;

        public NewMerchandiseAppearedCommandHandler(IMediator mediator, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException($"{nameof(unitOfWork)}");
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)}");
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
                    await _mediator.Send(new GiveOutOrderCommand { order = item }, cancellationToken);
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
    }
}
