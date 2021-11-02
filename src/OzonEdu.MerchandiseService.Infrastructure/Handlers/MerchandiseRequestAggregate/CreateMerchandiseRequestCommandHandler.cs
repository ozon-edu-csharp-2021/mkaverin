using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchandiseRequestAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.Exceptions;
using OzonEdu.StockApi.Infrastructure.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.MerchandiseRequestAggregate
{
    internal class CreateMerchandiseRequestCommandHandler : IRequestHandler<CreateMerchandiseRequestCommand, int>
    {

        public readonly IMerchandiseRequestRepository _merchandiseRequestRepository;
        public readonly IMerchPackRepository _merchPackRepository;
        private readonly IMediator _mediator;
        public CreateMerchandiseRequestCommandHandler(IMediator mediator, IMerchandiseRequestRepository merchandiseRequestRepository, IMerchPackRepository merchPackRepository)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository ??
                                         throw new ArgumentNullException($"{nameof(merchandiseRequestRepository)}");
            _merchPackRepository = merchPackRepository ??
                                       throw new ArgumentNullException($"{nameof(merchPackRepository)}");
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateMerchandiseRequestCommand request, CancellationToken cancellationToken)
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
            MerchandiseRequest requestMR = new(date: new(DateTimeOffset.UtcNow),
                                                idEmployee: new(request.IdEmployee),
                                                merchPack: merchPack,
                                                source: request.Sourse);

            MerchandiseRequest createdMerchandiseRequest = await _merchandiseRequestRepository.CreateAsync(requestMR, cancellationToken);
            await _merchandiseRequestRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return createdMerchandiseRequest.Id;
        }
    }
}
