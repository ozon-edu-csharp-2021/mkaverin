using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class ClothingSizeType : Enumeration
    {
        public static ClothingSizeType XS = new(1, nameof(XS));
        public static ClothingSizeType S = new(2, nameof(S));
        public static ClothingSizeType M = new(3, nameof(M));
        public static ClothingSizeType L = new(4, nameof(L));
        public static ClothingSizeType XL = new(5, nameof(XL));
        public static ClothingSizeType XXL = new(6, nameof(XXL));

        public ClothingSizeType(int id, string name) : base(id, name)
        {
        }

    }
}
