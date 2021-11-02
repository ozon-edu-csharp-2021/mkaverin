using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchandiseRequestAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.MerchandiseRequestAggregate
{
    public class CheckGiveOutMerchByIdEmployeeQueryHendler : IRequestHandler<CheckGiveOutMerchByIdEmployeeQuery, bool>
    {
        private const int DAYSYEAR = 366;

        public readonly IMerchandiseRequestRepository _merchandiseRequestRepository;

        public CheckGiveOutMerchByIdEmployeeQueryHendler(IMerchandiseRequestRepository merchandiseRequestRepository)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository ??
                                         throw new ArgumentNullException($"{nameof(merchandiseRequestRepository)}");
        }

        public async Task<bool> Handle(CheckGiveOutMerchByIdEmployeeQuery request, CancellationToken cancellationToken)
        {
            EmployeeId employeeIdRequest = new(request.EmployeeId);
            List<MerchandiseRequest> merchandiseRequests = await _merchandiseRequestRepository.GetAllMerchandiseRequestByEmployeeIdAsync(employeeIdRequest);

            IEnumerable<MerchandiseRequest> checkGiveOut = merchandiseRequests
                .Where(r => (DateTimeOffset.UtcNow - r.DeliveryDate.Value).TotalDays < DAYSYEAR)
                .Where(r => r.MerchPack.MerchType == request.MerchType && r.Status == Status.Done);

            return checkGiveOut.Any();
        }
    }
}
