using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    internal class NewMerchandiseAppearedCommandHandler : IRequestHandler<NewMerchandiseAppearedCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public NewMerchandiseAppearedCommandHandler(IMediator mediator, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(NewMerchandiseAppearedCommand request, CancellationToken cancellationToken)
        {
            var ordersInStatusInQueue = await _orderRepository.GetAllOrderInStatusAsync(new(StatusType.InQueue.Id), cancellationToken);
            

            /*
               Не стал делать проверку того что пришло, а просто пытаюсь зарезервировать всех кто в очереди.
                  1. Так как данные со stockApi приходят без кол-во, и в разрезе sku и проверка очень громоздкая выходит. 
                  2. Есть вариант что то что было на складе + то что пришло. Покроет потребность в выдаче. Данный кейс я не могу проверить на своей стороне
                  2. Я все равно делаю большую проверку на своей стороне по типу в момент попытки заразервировать товар у stockApi
                  3. Всегда можно переделать если будут проблемы с производительностью.
             */

            foreach (var item in ordersInStatusInQueue)
            {
                switch (item.Source.Type.Id)
                {
                    case 1:
                        
                        var isAvailable = await _mediator.Send(new GetStockItemsAvailabilityQuery { MerchItems = item.MerchPack.MerchItems, Size = item.ClothingSize }, cancellationToken);
                        if (isAvailable is not null)
                        {
                            item.ChangeStatusNotified();
                            await _orderRepository.UpdateAsync(item, cancellationToken);
                        }
                        break;
                    case 2:
                        await _unitOfWork.StartTransaction(cancellationToken);
                        await _mediator.Send(new GiveOutOrderCommand { order = item }, cancellationToken);
                        await _orderRepository.UpdateAsync(item, cancellationToken);
                        break;
                    default:
                        throw new NoSourceException("No source");
                }
            }
            return Unit.Value;
        }
    }
}
