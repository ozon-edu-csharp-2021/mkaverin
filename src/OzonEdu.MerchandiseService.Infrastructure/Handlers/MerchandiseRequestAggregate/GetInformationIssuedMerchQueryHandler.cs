using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchandiseRequestAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.MerchandiseRequestAggregate
{
    public class GetInformationIssuedMerchQueryHandler : IRequestHandler<GetInformationIssuedMerchQuery, GetInformationIssuedMerchQueryResponse>
    {
        public readonly IMerchandiseRequestRepository _merchandiseRequestRepository;

        public GetInformationIssuedMerchQueryHandler(IMerchandiseRequestRepository merchandiseRequestRepository)
        {
            _merchandiseRequestRepository = merchandiseRequestRepository ??
                                         throw new ArgumentNullException($"{nameof(merchandiseRequestRepository)}");
        }

        public async Task<GetInformationIssuedMerchQueryResponse> Handle(GetInformationIssuedMerchQuery request, CancellationToken cancellationToken)
        {
            EmployeeId employeeIdRequest = new(request.EmployeeId);
            List<MerchandiseRequest> merchandiseRequests = await _merchandiseRequestRepository.GetAllMerchandiseRequestByEmployeeIdAsync(employeeIdRequest);
            #region Mapping
            GetInformationIssuedMerchQueryResponse result = new()
            {
                DeliveryMerch = new DeliveryMerch[merchandiseRequests.Count]
            };
            for (int i = 0; i < merchandiseRequests.Count; i++)
            {
                if (merchandiseRequests[i].Status == Status.Done)
                {
                    result.DeliveryMerch[i] = new DeliveryMerch()
                    {
                        DeliveryDate = merchandiseRequests[i].DeliveryDate.Value,
                        merchPack = merchandiseRequests[i].MerchPack
                    };
                }
            }
            #endregion
            return result;
        }
    }
}