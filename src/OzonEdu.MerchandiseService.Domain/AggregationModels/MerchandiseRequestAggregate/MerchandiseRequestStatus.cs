using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate
{
    public class MerchandiseRequestStatus : Enumeration
    {
        public static MerchandiseRequestStatus New = new(1, nameof(New));
        public static MerchandiseRequestStatus InQueue = new(2, nameof(InQueue));
        public static MerchandiseRequestStatus Done = new(3, nameof(Done));
        public static MerchandiseRequestStatus Notified = new(4, nameof(Notified));
        public MerchandiseRequestStatus(int id, string name) : base(id, name)
        {
        }
    }
}