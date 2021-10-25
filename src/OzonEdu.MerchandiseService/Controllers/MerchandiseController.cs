using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("v1/api/merch")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        public MerchandiseController()
        {

        }

        [HttpPost("requestmerch")]
        public async Task<RequestMerchResponseDto> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        [HttpPost("getinfomerch")]
        public async Task<GetInfoMerchResponseDto> GetInfoMerch(GetInfoMerchRequestDto requestDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
