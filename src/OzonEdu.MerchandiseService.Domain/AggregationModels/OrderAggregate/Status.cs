using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate
{
    public class Status : Enumeration
    {
        public static Status New = new(1, nameof(New));
        public static Status InQueue = new(2, nameof(InQueue));
        public static Status Done = new(3, nameof(Done));
        public static Status Notified = new(4, nameof(Notified));
        public Status(int id, string name) : base(id, name)
        {
        }
    }
}