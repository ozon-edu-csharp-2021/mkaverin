using Grpc.Core;
using OzonEdu.MerchandiseService.Grpc;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.GrpcServices
{
    public class MerchandiseGrpService : MerchandiseGrpc.MerchandiseGrpcBase
    {
        public MerchandiseGrpService()
        {

        }

        public override Task<GetInfoMerchResponse> GetInfoMerch(GetInfoMerchRequest request, ServerCallContext context)
        {
            return base.GetInfoMerch(request, context);
        }

        public override Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
        {
            return base.RequestMerch(request, context);
        }
    }
}
