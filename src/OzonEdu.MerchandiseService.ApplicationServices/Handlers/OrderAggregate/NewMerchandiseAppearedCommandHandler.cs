using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
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
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(NewMerchandiseAppearedCommand request, CancellationToken cancellationToken)
        {
            int ex = SourceType.External.Id;
            var orders = await _orderRepository.GetAllOrderInStatusAsync(new(StatusType.InQueue.Id), cancellationToken);
            var filtersAndSortOrders = orders
                 .Where(o => o.MerchPack.MerchItems.Any(sku => request.Items.Contains(sku)))
                 .OrderBy(o => o.Source)
                 .ToList();
            foreach (var item in filtersAndSortOrders)
            {
                switch (item.Source.Type.Id)
                {
                    case 1:
                        await _mediator.Send(new GiveOutOrderCommand { order = item }, cancellationToken);
                        break;
                    case 2:
                        if (GetStockItemsAvailability(item.MerchPack.MerchItems))
                            item.ChangeStatusNotified();
                        break;
                    default:
                        throw new NoSourceException("No source");
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
