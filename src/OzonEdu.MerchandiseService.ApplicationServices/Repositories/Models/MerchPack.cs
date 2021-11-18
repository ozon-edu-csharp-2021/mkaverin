using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Models
{
    public class MerchPack
    {
        public long id { get; set; }
        public int merch_type_id { get; set; }
        public string merch_items { get; set; }
    }
}