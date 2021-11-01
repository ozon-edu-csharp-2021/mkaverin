using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate
{
    public class MerchandiseRequestSource : Enumeration
    {
        public static MerchandiseRequestSource External = new(1, nameof(External));
        public static MerchandiseRequestSource Internal = new(2, nameof(Internal));

        public MerchandiseRequestSource(int id, string name) : base(id, name)
        {
        }
    }
}