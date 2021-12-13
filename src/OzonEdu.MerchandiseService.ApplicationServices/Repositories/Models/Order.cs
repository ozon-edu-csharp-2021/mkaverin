using System;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Models
{
    public class Order
    {
        public long id { get; set; }
        public DateTimeOffset creation_date { get; set; }
        public string employee_email { get; set; }
        public string employee_name { get; set; }
        public string manager_email { get; set; }
        public string manager_name { get; set; }
        public int clothing_size { get; set; }
        public long source_id { get; set; }
        public long status_id { get; set; }
        public DateTimeOffset? delivery_date { get; set; }
    }
}