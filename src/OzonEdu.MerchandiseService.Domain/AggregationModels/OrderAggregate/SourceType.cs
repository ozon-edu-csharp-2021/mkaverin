using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class SourceType : Enumeration
    {
        public static SourceType External = new(1, nameof(External));
        public static SourceType Internal = new(2, nameof(Internal));

        protected SourceType(int id, string name) : base(id, name)
        {
        }
    }
}