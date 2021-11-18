using System;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Models
{
    public class Order
    {
        public long id { get; set; }
        public DateTimeOffset creation_date { get; set; }
        public long employee_id { get; set; }
        public long source_id { get; set; }
        public long status_id { get; set; }
        public DateTimeOffset? delivery_date { get; set; }
    }
}