using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class Source : Enumeration
    {
        public static Source External = new(1, nameof(External));
        public static Source Internal = new(2, nameof(Internal));

        public Source(int id, string name) : base(id, name)
        {
        }
    }
}