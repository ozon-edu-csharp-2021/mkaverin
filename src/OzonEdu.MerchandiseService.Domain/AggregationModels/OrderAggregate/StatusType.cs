using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class StatusType : Enumeration
    {
        public static StatusType New = new(1, nameof(New).ToLowerInvariant());
        public static StatusType InQueue = new(2, nameof(InQueue).ToLowerInvariant());
        public static StatusType Done = new(3, nameof(Done).ToLowerInvariant());
        public static StatusType Notified = new(4, nameof(Notified).ToLowerInvariant());

        protected StatusType(int id, string name) : base(id, name)
        {

        }
    }
}