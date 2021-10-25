using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.HttpClient
{
    public interface IMerchandiseHttpClient
    {
        Task<List<StockItemResponse>> V1GetAll(CancellationToken token);
    }
}
