using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchandiseRequestAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.Exceptions;
using OzonEdu.StockApi.Domain.Exceptions.StockItemAggregate;
using OzonEdu.StockApi.Infrastructure.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.MerchandiseRequestAggregate
{
    internal class GiveOutMerchandiseRequestCommandHandler : IRequestHandler<GiveOutMerchandiseRequestCommand, bool>
    {
        public readonly IMerchandiseRequestRepository _merchandiseRequestRepository;
        private readonly IMediator _mediator;

        public GiveOutMerchandiseRequestCommandHandler(IMediator mediator, IMerchandiseRequestRepository merchandiseRequestRepository)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository ??
                                         throw new ArgumentNullException($"{nameof(merchandiseRequestRepository)}");
            _mediator = mediator;
        }

        public async Task<bool> Handle(GiveOutMerchandiseRequestCommand request, CancellationToken cancellationToken)
        {
            MerchandiseRequest merchandiseRequest = await _merchandiseRequestRepository.FindByIdAsync(request.MerchandiseRequestId);
            if (merchandiseRequest is not null)
            {
                throw new NoMerchandiseRequestException($"No MerchandiseRequest with id {request.MerchandiseRequestId}");
            }

            CheckGiveOutMerchByIdEmployeeQuery query = new()
            {
                EmployeeId = merchandiseRequest.EmployeeId.Value,
                MerchType = merchandiseRequest.MerchPack.MerchType
            };
            bool checkGiveOut = await _mediator.Send(query, cancellationToken);
            if (checkGiveOut)
            {
                throw new MerchAlreadyGiveOutException("The employee has already been issued merch");
            }

            //Обращаемся к сервису StockApi узнаем есть ли товар на складе
            //Если нету то вызываем 
            merchandiseRequest.ChangeStatusToInQueue();
            return false;

            //Если есть 
            //Обращаемся к сервису StockApi и резервируем товар на складе
            //вызываем 
            merchandiseRequest.ChangeStatusToDone(DateTimeOffset.UtcNow);
            return true;
        }
    }
}
