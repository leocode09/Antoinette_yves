using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class PartnerService
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }
        public int BusinessServiceId { get; set; }
        public BusinessService BusinessService { get; set; }
        public bool IsActive { get; set; }
        public int? Seats { get; set; }
        public int? MinDuration { get; set; }
        public int Price { get; set; }
        public DateTime CreatedOnDate { get; set; }
    }
}
